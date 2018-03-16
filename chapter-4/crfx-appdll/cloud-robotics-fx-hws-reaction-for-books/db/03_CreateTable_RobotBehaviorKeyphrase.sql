CREATE TABLE [RBApp].[RobotBehaviorKeyphrase](
	[Id] bigint IDENTITY NOT NULL,
	[SynAppsId] int NOT NULL,
	[SynAppsDeviceId] NVARCHAR(255) NULL,
	[Keyphrase] [nvarchar](100) NULL,
	[KeyphraseReply] [nvarchar](100) NULL,
	[Status] [nvarchar](40) NOT NULL DEFAULT 'InActive',
	[IsSynAppsLinked] BIT NOT NULL DEFAULT 0,
	[IsDeleted] BIT NULL,
	[CreatedAt] DATETIME NOT NULL,
	[UpdatedAt] DATETIME NULL
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)
)

CREATE INDEX [IDX_RobotBehaviorKeyphraseSynAppsDeviceIdKeyphrase] ON [RBApp].[RobotBehaviorKeyphrase]
(
    [SynAppsDeviceId] ASC,
    [Keyphrase] ASC
)
WITH (DATA_COMPRESSION=PAGE)

GO
