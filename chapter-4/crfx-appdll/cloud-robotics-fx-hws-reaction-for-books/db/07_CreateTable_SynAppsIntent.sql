CREATE TABLE [RBApp].[SynAppsIntent](
	[Id] bigint IDENTITY NOT NULL,
    [SynAppsDeviceId] NVARCHAR(255) NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Entity] [nvarchar](255) NULL,
	[ReactionBody] [nvarchar](4000) NULL,
	[IsSynAppsLinked] BIT NOT NULL DEFAULT 0,
	[IsDeleted] BIT NOT NULL DEFAULT 0,
	[CreatedAt] DATETIME NULL,
	[UpdatedAt] DATETIME NULL
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)
)

CREATE INDEX [IDX_SynAppsIntentSynAppsDeviceId] ON [RBApp].[SynAppsIntent]
(
    [SynAppsDeviceId] ASC
)
WITH (DATA_COMPRESSION=PAGE)


INSERT INTO [RBApp].[SynAppsIntent]
(
    [SynAppsDeviceId],
	[Name],
	[Entity],
	[ReactionBody],
	[CreatedAt]
)
VALUES
('yourdeviceid','質問','天気','{"Talk": "\\rspd=110\\\\vct=135\\ 気持ちはいつでも快晴です！"}',sysdatetime()),
('yourdeviceid','質問','ラーメン','{"Talk": "\\rspd=110\\\\vct=135\\ 駅前のお店がおススメです"}',sysdatetime()),
('yourdeviceid','質問','トイレ','{"Talk": "\\rspd=110\\\\vct=135\\ 入り口を出て、右手にあります"}',sysdatetime()),
('yourdeviceid','質問','昼食,メニュー','{"Talk": "\\rspd=110\\\\vct=135\\ メニューはありません"}',sysdatetime()),
('yourdeviceid','質問',null,'{"Talk": "\\rspd=110\\\\vct=135\\ ${target} 。。。ごめんなさい。まだ勉強中で、お答えできません。スタッフまでお尋ねください。"}',sysdatetime());

GO
