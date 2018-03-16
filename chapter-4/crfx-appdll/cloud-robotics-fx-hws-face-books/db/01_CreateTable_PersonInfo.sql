CREATE TABLE [RBApp].[PersonInfo](
	[Id] bigint IDENTITY NOT NULL,
	[AccountId] int NULL,
	[CmsGroupId] int NULL,
	[CmsPersonId] int NULL,
	[PersonGroupId] [nvarchar](40) NULL,
	[PersonId] [nvarchar](40) NOT NULL,
	[PersonName] [nvarchar](100) NULL,
	[PersonNameYomi] [nvarchar](100) NULL,
	[VisitCount] int NOT NULL DEFAULT 0,
	[VisitCountOfDay] int NOT NULL DEFAULT 0,
	[LastDetectedAt] DATETIME NULL,
	[IsDeleted] BIT NULL,
	[CreatedAt] DATETIME NULL,
	[UpdatedAt] DATETIME NULL
PRIMARY KEY CLUSTERED 
(
	[PersonId] ASC
)
)

ALTER TABLE [RBApp].[PersonInfo] ALTER COLUMN [PersonId] [nvarchar](255) NOT NULL;

GO
