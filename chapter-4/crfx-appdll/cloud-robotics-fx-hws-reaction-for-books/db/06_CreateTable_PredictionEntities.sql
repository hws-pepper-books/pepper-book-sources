CREATE TABLE [RBApp].[PredictionEntities](
	[Id] bigint IDENTITY NOT NULL,
	[SynAppsDeviceId] NVARCHAR(255) NULL,
	[LuisExampleId] bigint NOT NULL,
	[EntityName] NVARCHAR(255) NULL,
	[EntityValue] NVARCHAR(255) NULL,
	[CreatedAt] DATETIME NOT NULL,
	[UpdatedAt] DATETIME NOT NULL
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)
)

CREATE INDEX [IDX_PredictionEntitySynAppsDeviceIdLuisExampleId] ON [RBApp].[PredictionEntities]
(
    [SynAppsDeviceId] ASC,
    [LuisExampleId] ASC
)
WITH (DATA_COMPRESSION=PAGE)

GO
