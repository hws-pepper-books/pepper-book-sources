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
('yourdeviceid',1,'Active','first_contact','{"Id":1,"ActionType":"first_contact","RobotTalk":{"TalkType":"robot_talk_pattern","Talk_1":"あ、もしかして、お会いするのは初めてでしょうか？","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyYes":{"TalkType":"person_reply_yes","Talk_1":null,"Talk_2":null,"Talk_3":null,"Talk_4":null,"Talk_5":null,"Talk_6":null,"Talk_7":null,"Talk_8":null,"Talk_9":null,"Talk_10":null},"PersonReplyNg":{"TalkType":"person_reply_ng","Talk_1":null,"Talk_2":null,"Talk_3":null,"Talk_4":null,"Talk_5":null,"Talk_6":null,"Talk_7":null,"Talk_8":null,"Talk_9":null,"Talk_10":null},"PersonReplyOther":{"TalkType":"person_reply_other","Talk_1":null,"Talk_2":null,"Talk_3":null,"Talk_4":null,"Talk_5":null,"Talk_6":null,"Talk_7":null,"Talk_8":null,"Talk_9":null,"Talk_10":null},"PersonReplyOtherKeyword":""}',sysdatetime()),
('yourdeviceid',2,'Active','has_keyphrase','{"Id":2,"ActionType":"has_keyphrase","RobotTalk":{"TalkType":"robot_talk_pattern","Talk_1":"あ、前に${keyphrase}についてお話しましたね。${keyphrase}って${keyphrase_reply}だって知ってましたか？","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyYes":{"TalkType":"person_reply_yes","Talk_1":"さすがーご存じですよね。良かったら、他にも聞いてください。","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyNg":{"TalkType":"person_reply_ng","Talk_1":"そうですかぁ。じゃあ少しはお役にたてたかもしれませんね。良かったら、他にも聞いてください。","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyOther":{"TalkType":"person_reply_other","Talk_1":"そうですかぁ。じゃあ少しはお役にたてたかもしれませんね。良かったら、他にも聞いてください。","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyOtherKeyword":""}',sysdatetime()),
('yourdeviceid',3,'Active','today','{"Id":3,"ActionType":"today","RobotTalk":{"TalkType":"robot_talk_pattern","Talk_1":"あ、今日お会いするのは${visit_count}回目ですね。とっても嬉しいです。これからお帰りですか？\n","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyYes":{"TalkType":"person_reply_yes","Talk_1":"お疲れ様でした。次に来られたときも声をかけてくださいね。","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyNg":{"TalkType":"person_reply_ng","Talk_1":"そうなんですね。じゃあお帰りの前にまた会えるかもしれませんね。","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyOther":{"TalkType":"person_reply_other","Talk_1":"できれば、お帰りの前に、一言お声掛けいただけるとうれしいなあ。","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyOtherKeyword":""}',sysdatetime()),
('yourdeviceid',4,'Active','past_days_clear','{"Id":4,"ActionType":"past_days_clear","RobotTalk":{"TalkType":"robot_talk_pattern","Talk_1":"あ、${days}日ぶりですね。またお会いできてうれしいです。前回は${last_weather}でしたけど、今日は晴れてて気持ちいいですね。これからお帰りですか？\n","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyYes":{"TalkType":"person_reply_yes","Talk_1":"お疲れ様でした。次に来られたときも声をかけてくださいね。","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyNg":{"TalkType":"person_reply_ng","Talk_1":"そうなんですね。じゃあお帰りの前にまた会えるかもしれませんね。\n","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyOther":{"TalkType":"person_reply_other","Talk_1":"そうなんですね。お帰りの前に、一言お声掛けいただけるとうれしいなあ。\n","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyOtherKeyword":""}',sysdatetime()),
('yourdeviceid',5,'Active','past_days_none_clear','{"Id":5,"ActionType":"past_days_none_clear","RobotTalk":{"TalkType":"robot_talk_pattern","Talk_1":"あ、${days}日ぶりですね。またお会いできてうれしいです。今日は天気がよくないですねー。これからお帰りですか？","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyYes":{"TalkType":"person_reply_yes","Talk_1":"お疲れ様でした。次に来られたときも声をかけてくださいね。\n","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyNg":{"TalkType":"person_reply_ng","Talk_1":"そうなんですね。じゃあお帰りの前にまた会えるかもしれませんね。\n","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyOther":{"TalkType":"person_reply_other","Talk_1":"そうなんですね。お帰りの前に、一言お声掛けいただけるとうれしいなあ。","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyOtherKeyword":""}',sysdatetime()),
('yourdeviceid',6,'Active','free_talk','{"Id":6,"ActionType":"free_talk","RobotTalk":{"TalkType":"robot_talk_pattern","Talk_1":"あ、${visit_count}日ぶりですね。またお会いできてうれしいです。今日はどんなトレーニングをされるんですか？\n","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyYes":{"TalkType":"person_reply_yes","Talk_1":null,"Talk_2":null,"Talk_3":null,"Talk_4":null,"Talk_5":null,"Talk_6":null,"Talk_7":null,"Talk_8":null,"Talk_9":null,"Talk_10":null},"PersonReplyNg":{"TalkType":"person_reply_ng","Talk_1":null,"Talk_2":null,"Talk_3":null,"Talk_4":null,"Talk_5":null,"Talk_6":null,"Talk_7":null,"Talk_8":null,"Talk_9":null,"Talk_10":null},"PersonReplyOther":{"TalkType":"person_reply_other","Talk_1":"${keyword}なんですねー。${keyword}って体にいいですよねー。体にいいといえば、ゴルフとかも人気があるみたいですよ。よかったらお試しください。","Talk_2":"","Talk_3":"","Talk_4":"","Talk_5":"","Talk_6":"","Talk_7":"","Talk_8":"","Talk_9":"","Talk_10":""},"PersonReplyOtherKeyword":"ヨガ、プール"}',sysdatetime());

GO
