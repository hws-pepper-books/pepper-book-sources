CREATE TABLE [RBApp].[RobotBehaviorToPersons](
	[Id] bigint IDENTITY NOT NULL,
	[DeviceId] [nvarchar](100) NOT NULL,
	[PersonId] [nvarchar](40) NOT NULL,
	[RobotBehaviorId] int NOT NULL,
	[Weather] [nvarchar](40) NOT NULL,
	[Status] [nvarchar](40) NOT NULL DEFAULT 'InActive',
	[IsDeleted] BIT NULL,
	[CreatedAt] DATETIME NOT NULL,
	[UpdatedAt] DATETIME NULL
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)
)

CREATE INDEX [IDX_RobotBehaviorToPersonsDeviceIdPersonId] ON [RBApp].[RobotBehaviorToPersons]
(
    [DeviceId] ASC,
    [PersonId] ASC
)
WITH (DATA_COMPRESSION=PAGE)

CREATE INDEX [IDX_RobotBehaviorToPersonsPersonId] ON [RBApp].[RobotBehaviorToPersons]
(
    [PersonId] ASC
)
WITH (DATA_COMPRESSION=PAGE)

GO
