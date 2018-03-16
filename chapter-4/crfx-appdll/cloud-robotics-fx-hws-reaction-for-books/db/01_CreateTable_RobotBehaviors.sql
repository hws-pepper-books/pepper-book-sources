CREATE TABLE [RBApp].[RobotBehaviors](
	[Id] bigint IDENTITY NOT NULL,
	[SynAppsDeviceId] NVARCHAR(255) NULL,
	[SynAppsId] int NOT NULL,
	[Status] [nvarchar](40) NOT NULL DEFAULT 'InActive',
	[ActionType] [nvarchar](40) NULL,
	[ActionBody] [nvarchar](4000) NULL,
	[IsSynAppsLinked] BIT NOT NULL DEFAULT 0,
	[IsDeleted] BIT NOT NULL DEFAULT 0,
	[CreatedAt] DATETIME NOT NULL,
	[UpdatedAt] NULL
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)
)

CREATE INDEX [IDX_RobotBehaviorsSynAppsDeviceIdSynAppsId] ON [RBApp].[RobotBehaviors]
(
    [SynAppsDeviceId] ASC,
    [SynAppsId] ASC
)
WITH (DATA_COMPRESSION=PAGE)


INSERT INTO [RBApp].[RobotBehaviors]
(
	[SynAppsDeviceId],
	[SynAppsId],
	[Status],
	[ActionType],
	[ActionBody],
	[CreatedAt]
)
VALUES
('yourdeviceid',1,'Active','first_contact','{"Id":1,"ActionType":"first_contact","RobotTalk":{"TalkType":"robot_talk_pattern","Talk_1":"���A���������āA�������̂͏��߂Ăł��傤���H","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyYes":{"TalkType":"person_reply_yes","Talk_1":null,"Talk_2":null,"Talk_3":null,"Talk_4":null,"Talk_5":null,"Talk_6":null,"Talk_7":null,"Talk_8":null,"Talk_9":null,"Talk_10":null},"PersonReplyNg":{"TalkType":"person_reply_ng","Talk_1":null,"Talk_2":null,"Talk_3":null,"Talk_4":null,"Talk_5":null,"Talk_6":null,"Talk_7":null,"Talk_8":null,"Talk_9":null,"Talk_10":null},"PersonReplyOther":{"TalkType":"person_reply_other","Talk_1":null,"Talk_2":null,"Talk_3":null,"Talk_4":null,"Talk_5":null,"Talk_6":null,"Talk_7":null,"Talk_8":null,"Talk_9":null,"Talk_10":null},"PersonReplyOtherKeyword":""}',sysdatetime()),
('yourdeviceid',2,'Active','has_keyphrase','{"Id":2,"ActionType":"has_keyphrase","RobotTalk":{"TalkType":"robot_talk_pattern","Talk_1":"���A�O��${keyphrase}�ɂ��Ă��b���܂����ˁB${keyphrase}����${keyphrase_reply}�����Ēm���Ă܂������H","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyYes":{"TalkType":"person_reply_yes","Talk_1":"�������[�������ł���ˁB�ǂ�������A���ɂ������Ă��������B","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyNg":{"TalkType":"person_reply_ng","Talk_1":"�����ł������B���Ⴀ�����͂����ɂ��Ă���������܂���ˁB�ǂ�������A���ɂ������Ă��������B","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyOther":{"TalkType":"person_reply_other","Talk_1":"�����ł������B���Ⴀ�����͂����ɂ��Ă���������܂���ˁB�ǂ�������A���ɂ������Ă��������B","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyOtherKeyword":""}',sysdatetime()),
('yourdeviceid',3,'Active','today','{"Id":3,"ActionType":"today","RobotTalk":{"TalkType":"robot_talk_pattern","Talk_1":"���A�����������̂�${visit_count}��ڂł��ˁB�Ƃ��Ă��������ł��B���ꂩ�炨�A��ł����H\n","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyYes":{"TalkType":"person_reply_yes","Talk_1":"�����l�ł����B���ɗ���ꂽ�Ƃ������������Ă��������ˁB","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyNg":{"TalkType":"person_reply_ng","Talk_1":"�����Ȃ�ł��ˁB���Ⴀ���A��̑O�ɂ܂���邩������܂���ˁB","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyOther":{"TalkType":"person_reply_other","Talk_1":"�ł���΁A���A��̑O�ɁA�ꌾ�����|������������Ƃ��ꂵ���Ȃ��B","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyOtherKeyword":""}',sysdatetime()),
('yourdeviceid',4,'Active','past_days_clear','{"Id":4,"ActionType":"past_days_clear","RobotTalk":{"TalkType":"robot_talk_pattern","Talk_1":"���A${days}���Ԃ�ł��ˁB�܂�����ł��Ă��ꂵ���ł��B�O���${last_weather}�ł������ǁA�����͐���ĂċC���������ł��ˁB���ꂩ�炨�A��ł����H\n","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyYes":{"TalkType":"person_reply_yes","Talk_1":"�����l�ł����B���ɗ���ꂽ�Ƃ������������Ă��������ˁB","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyNg":{"TalkType":"person_reply_ng","Talk_1":"�����Ȃ�ł��ˁB���Ⴀ���A��̑O�ɂ܂���邩������܂���ˁB\n","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyOther":{"TalkType":"person_reply_other","Talk_1":"�����Ȃ�ł��ˁB���A��̑O�ɁA�ꌾ�����|������������Ƃ��ꂵ���Ȃ��B\n","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyOtherKeyword":""}',sysdatetime()),
('yourdeviceid',5,'Active','past_days_none_clear','{"Id":5,"ActionType":"past_days_none_clear","RobotTalk":{"TalkType":"robot_talk_pattern","Talk_1":"���A${days}���Ԃ�ł��ˁB�܂�����ł��Ă��ꂵ���ł��B�����͓V�C���悭�Ȃ��ł��ˁ[�B���ꂩ�炨�A��ł����H","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyYes":{"TalkType":"person_reply_yes","Talk_1":"�����l�ł����B���ɗ���ꂽ�Ƃ������������Ă��������ˁB\n","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyNg":{"TalkType":"person_reply_ng","Talk_1":"�����Ȃ�ł��ˁB���Ⴀ���A��̑O�ɂ܂���邩������܂���ˁB\n","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyOther":{"TalkType":"person_reply_other","Talk_1":"�����Ȃ�ł��ˁB���A��̑O�ɁA�ꌾ�����|������������Ƃ��ꂵ���Ȃ��B","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyOtherKeyword":""}',sysdatetime()),
('yourdeviceid',6,'Active','free_talk','{"Id":6,"ActionType":"free_talk","RobotTalk":{"TalkType":"robot_talk_pattern","Talk_1":"���A${visit_count}���Ԃ�ł��ˁB�܂�����ł��Ă��ꂵ���ł��B�����͂ǂ�ȃg���[�j���O��������ł����H\n","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyYes":{"TalkType":"person_reply_yes","Talk_1":null,"Talk_2":null,"Talk_3":null,"Talk_4":null,"Talk_5":null,"Talk_6":null,"Talk_7":null,"Talk_8":null,"Talk_9":null,"Talk_10":null},"PersonReplyNg":{"TalkType":"person_reply_ng","Talk_1":null,"Talk_2":null,"Talk_3":null,"Talk_4":null,"Talk_5":null,"Talk_6":null,"Talk_7":null,"Talk_8":null,"Talk_9":null,"Talk_10":null},"PersonReplyOther":{"TalkType":"person_reply_other","Talk_1":"${keyword}�Ȃ�ł��ˁ[�B${keyword}���đ̂ɂ����ł���ˁ[�B�̂ɂ����Ƃ����΁A�S���t�Ƃ����l�C������݂����ł���B�悩�����炨�������������B","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyOtherKeyword":"���K�A�v�[��"}',sysdatetime());

GO
