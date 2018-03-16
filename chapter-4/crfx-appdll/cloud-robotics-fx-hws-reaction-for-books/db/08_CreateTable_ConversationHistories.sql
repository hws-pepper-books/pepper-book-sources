CREATE TABLE [RBApp].[ConversationHistories](
	[Id] bigint IDENTITY NOT NULL,
	[DeviceId] NVARCHAR(255) NULL,
	[FromUserId] NVARCHAR(255) NULL,
	[FromMessage] [nvarchar](4000) NOT NULL,
	[ToMessage] [nvarchar](4000) NOT NULL,
	[Intent] NVARCHAR(255) NULL,
	[Score] Float NOT NULL DEFAULT 0,
	[SynAppsAccountId] bigint NOT NULL DEFAULT 0,
	[SynAppsAccountName] NVARCHAR(255) NULL,
	[SynAppAssetId] bigint NOT NULL DEFAULT 0,
	[Status] NVARCHAR(10) NOT NULL DEFAULT 'None',
	[IsSynAppsLinked] BIT NOT NULL DEFAULT 0,
	[CreatedAt] DATETIME NOT NULL,
	[UpdatedAt] DATETIME NOT NULL
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)
)

CREATE INDEX [IDX_ConversationHistoryDeviceId] ON [RBApp].[ConversationHistories]
(
    [DeviceId] ASC
)
WITH (DATA_COMPRESSION=PAGE)
GO
