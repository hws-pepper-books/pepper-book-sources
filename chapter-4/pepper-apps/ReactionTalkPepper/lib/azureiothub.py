# -*- coding: utf-8 -*-

import time, datetime
import urllib, urllib2
import hmac, hashlib
import base64
import json

"""
    IoTデバイス
"""
class Device():
    def __init__(self, hostname, deviceId, sharedAccessKey):
        self.hostname = hostname
        self.deviceId = deviceId
        self.sharedAccessKey = sharedAccessKey
    
    # Security Access Sigunature(SAS)を作成する
    # http://www.jokecamp.com/blog/examples-of-creating-base64-hashes-using-hmac-sha256-in-different-languages/#python
    #
    def create_sas_token(self, expireTerm=3600):
        # 有効期限は1時間とする。
        expiry = time.mktime(datetime.datetime.now().utctimetuple())+expireTerm
        expiry = str(int(expiry))
        # quoteだと、スラッシュがデフォルトでエンコードされない対象となっているため、safeを空にする。
        uri = "{hostname}/devices/{deviceId}".format(hostname=self.hostname, deviceId=self.deviceId)
        uri_enc = urllib.quote(uri, safe='')
        signature = uri_enc + '\n' + expiry
        
        # SharedAccessKeyはBase64でエンコードされているため、デコードする。
        k = bytes(base64.b64decode(self.sharedAccessKey))
        v = bytes(signature)
        
        # SignatureはHMAC-SHA256で処理する。
        sig_enc = base64.b64encode(hmac.new(k, v, digestmod=hashlib.sha256).digest())
        sig_enc = urllib.quote(sig_enc, safe='')
        
        # sknにkeyNameが入っていると認証エラーになる。
        token = 'SharedAccessSignature sig=' + sig_enc + '&se=' + expiry + '&sr=' + uri_enc + '&skn='
        
        return token

"""
    HTTPをプロトコルとして使用するデバイス
"""
class DeviceHttp(Device):
    def __init__(self, hostname, deviceId, sharedAccessKey):
        Device.__init__(self, hostname, deviceId, sharedAccessKey)
        self.api_version = '2016-02-03'
        # REST APIのURL
        self.restAPIs = {
            'send': '/devices/{deviceId}/messages/events'.format(deviceId=self.deviceId)
            ,'receive': '/devices/{deviceId}/messages/devicebound'.format(deviceId=self.deviceId)
            ,'complete': '/devices/{deviceId}/messages/devicebound/messageId'.format(deviceId=self.deviceId)
        }
        
        opener = urllib2.build_opener(
            urllib2.HTTPSHandler(debuglevel=1)
        )
        urllib2.install_opener(opener)
    
    # メッセージを送る
    # https://msdn.microsoft.com/ja-jp/library/azure/mt590784.aspx
    # 正常時：レスポンスコード204、本文なし
    def send(self, message):
        try:
            request = urllib2.Request(self.url('send'), message, self.headers('send'))
            urllib2.urlopen(request)
        except urllib2.HTTPError as he:
            print "\nFailed: " + str(he) + ", Response Code:" + str(he.code)
            return "\nFailed: " + str(he) + ", Response Code:" + str(he.code)
        except Exception as e:
            print "\nFailed: " + str(e)
            return "\nFailed: " + str(e)
        else:
            return "Success"
    
    def receive(self, complete=True):
        headers = None
        body = None
        resCode = None
        try:
            request = urllib2.Request(self.url('receive'), None, self.headers('receive'))
            response = urllib2.urlopen(request)
            headers = response.info()
            body = response.read()
            resCode = response.code
        except urllib2.HTTPError as he:
            print "\nFailed: " + str(he) + ", Response Code:" + str(he.code)
            return None
        except Exception as e:
            print "\nFailed: " + str(e)
            return None
        else:
            if resCode == 204: # No Content
                return ''
        
        # メッセージを完了させ、キューから削除する
        # ETagの値には、最初と最後にダブルクォーテーションが入るので、削除する。
        if complete and headers['ETag'] is not None:
            messageId = headers['ETag'].replace('\"','')
            self.complete(messageId)
        
        return json.loads(body)
    
    # メッセージを完了し、キューから削除する。
    # https://msdn.microsoft.com/ja-jp/library/azure/mt605155.aspx
    # 正常時：レスポンスコード204、本文なし
    def complete(self, messageId):
        try:
            request = urllib2.Request(self.url('complete', messageId), None, self.headers('complete'))
            request.get_method = lambda: 'DELETE'
            urllib2.urlopen(request)
        except urllib2.HTTPError as he:
            print "\nFailed: " + str(he) + ", Response Code:" + str(he.code)
            return False
        except Exception as e:
            print "\nFailed: " + str(e)
            return False
        else:
            return True
    
    # REST APIのURLを取得する
    #
    def url(self, apiType, messageId=None):
        url = 'https://{hostname}{api}?api-version={api_version}'.format(hostname=self.hostname, api=self.restAPIs[apiType], deviceId=self.deviceId, api_version=self.api_version)
        if messageId is not None:
            url = url.replace('messageId', messageId)
            
        return url
    
    # HTTPのヘッダを取得する
    #
    def headers(self, apiType):
        headers = {
            "Authorization": Device.create_sas_token(self)
            ,"iothub-to": self.restAPIs[apiType]
            ,"User-Agent": "azure-iot-device/1.0.0"
        }
        
        return headers
    
