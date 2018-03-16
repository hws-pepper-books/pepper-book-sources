CREATE TABLE [RBApp].[RobotBehaviorTalkLogs](
	[Id] bigint IDENTITY NOT NULL,
	[DeviceId] [nvarchar](100) NOT NULL,
	[PersonId] [nvarchar](40) NOT NULL,
	[RobotTalk] [nvarchar](1000) NOT NULL,
	[PersonTalk] [nvarchar](1000) NOT NULL,
	[PersonTalkKeyphrase] [nvarchar](200) NULL,
	[Status] [nvarchar](40) NOT NULL DEFAULT 'InActive',
	[IsDeleted] BIT NULL,
	[CreatedAt] DATETIME NOT NULL,
	[UpdatedAt] DATETIME NULL
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)
)

CREATE INDEX [IDX_RobotBehaviorTalkLogDeviceIdPersonId] ON [RBApp].[RobotBehaviorTalkLogs]
(
    [DeviceId] ASC,
    [PersonId] ASC
)
WITH (DATA_COMPRESSION=PAGE)

CREATE INDEX [IDX_RobotBehaviorTalkLogPersonId] ON [RBApp].[RobotBehaviorTalkLogs]
(
    [PersonId] ASC
)
WITH (DATA_COMPRESSION=PAGE)

GO
