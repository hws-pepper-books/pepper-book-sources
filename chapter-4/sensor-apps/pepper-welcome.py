#!/usr/bin/python
# -*- coding: utf-8 -*-

import os, sys, time, requests, json, logging, logging.handlers
import RPi.GPIO as GPIO
from datetime import datetime

LOG_FILENAME = '/var/log/pepper-welcome.log'
logger = logging.getLogger('PepperWelcomeLogger')
logger.setLevel(logging.DEBUG)
handler = logging.handlers.RotatingFileHandler(LOG_FILENAME, maxBytes=1000000, backupCount=3)
logger.addHandler(handler)

TRIG1 = 11
ECHO1 = 13
CHECK_DISTANCE = 2
CHECK_LIMIT_DISTANCE = 300

def setup():
    GPIO.setwarnings(False)     
    GPIO.setmode(GPIO.BOARD)
     
    GPIO.setup(TRIG1,GPIO.OUT)
    GPIO.setup(ECHO1,GPIO.IN)

def reading(TRIG, ECHO):
    signaloff = 0
    signalon = 0

    GPIO.output(TRIG, True)
    time.sleep(0.00001)
    GPIO.output(TRIG, False)

    starttime =  time.time()
    while GPIO.input(ECHO) == 0:
      signaloff = time.time()
      if ((signaloff - starttime) > 0.5):
        return 0
           
    while GPIO.input(ECHO) == 1:
      signalon = time.time()
      if ((signalon - starttime) > 0.5):
        return 0
 
    timepassed = signalon - signaloff
    distance = timepassed * 17000
    distance = float(int(distance * 10)) / 10

    if (distance < 3):
        return 0
    if (distance > 450):
        return 0
        
    return distance

def read_distance(distance):    
    GPIO.output(TRIG1, GPIO.LOW)
    time.sleep(0.1)

    distance[1] = 0
    while distance[1] == 0:
        distance[1] = reading(TRIG1, ECHO1)

    return

def send_message(talk):
    source_device_id = 'youriothubdeviceid'

    end_point = 'https://youriothubhost.azure-devices.net/devices/' + source_device_id + '/messages/events?api-version=2016-02-03'
    headers = {'Authorization' : 'SharedAccessSignature sr='}
    payload = {
        'RbHeader': {
            'RoutingType': 'D2D',
            'RoutingKeyword': 'Sensor',
            'AppId': 'OmotenashiPepperSample',
            'AppProcessingId': 'App',
            'MessageId': 'RobotSpeech',
            'MessageSeqno': 600,
            'SendDateTime': datetime.now().strftime('%Y-%m-%d %H:%M:%S'),
            'SourceDeviceId': source_device_id
        },
        'RbBody': {
            'talk': talk
        }
    }
    try:
        r = requests.post(end_point, headers=headers, data=json.dumps(payload), verify=False)
    except:
        pass

def sensor_main():
    distance = [0, 0, 0]
    check1 = []
    setup()

    try:
        while True:
            read_distance(distance)
            check1.append(distance[1])
            if (len(check1) > 5):
                check1.pop(0)
                if ((check1[0] - check1[2]) > CHECK_DISTANCE and
                        (check1[2] - check1[4]) > CHECK_DISTANCE and
                        (check1[0] < CHECK_LIMIT_DISTANCE and check1[2] < CHECK_LIMIT_DISTANCE and check1[4] < CHECK_LIMIT_DISTANCE)):

                    send_message("いらっしゃいませ")
                    time.sleep(2)
                
                logger.info("check1:" + str(check1[0])  + ", " + str(check1[1]) + ", " + str(check1[2]) + ", " + str(check1[3]) + ", " + str(check1[4]))
                del check1[:]
    except:
        GPIO.cleanup()
        pass

if __name__ == '__main__':
    sensor_main()        
