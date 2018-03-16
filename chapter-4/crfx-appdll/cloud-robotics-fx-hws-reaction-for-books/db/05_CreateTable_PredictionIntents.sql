CREATE TABLE [RBApp].[PredictionIntents](
	[Id] bigint IDENTITY NOT NULL,
	[SynAppsDeviceId] NVARCHAR(255) NULL,
	[LuisExampleId] bigint NOT NULL,
	[Intent] NVARCHAR(255) NULL,
	[CreatedAt] DATETIME NOT NULL,
	[UpdatedAt] DATETIME NOT NULL
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)
)

CREATE INDEX [IDX_PredictionIntentSynAppsDeviceIdLuisExampleId] ON [RBApp].[PredictionIntents]
(
    [SynAppsDeviceId] ASC,
    [LuisExampleId] ASC
)
WITH (DATA_COMPRESSION=PAGE)

CREATE INDEX [IDX_PredictionIntentDeviceIdIntent] ON [RBApp].[PredictionIntents]
(
    [SynAppsDeviceId] ASC,
    [Intent] ASC
)
WITH (DATA_COMPRESSION=PAGE)

GO
