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
('yourdeviceid','����','�V�C','{"Talk": "\\rspd=110\\\\vct=135\\ �C�����͂��ł������ł��I"}',sysdatetime()),
('yourdeviceid','����','���[����','{"Talk": "\\rspd=110\\\\vct=135\\ �w�O�̂��X�����X�X���ł�"}',sysdatetime()),
('yourdeviceid','����','�g�C��','{"Talk": "\\rspd=110\\\\vct=135\\ ��������o�āA�E��ɂ���܂�"}',sysdatetime()),
('yourdeviceid','����','���H,���j���[','{"Talk": "\\rspd=110\\\\vct=135\\ ���j���[�͂���܂���"}',sysdatetime()),
('yourdeviceid','����',null,'{"Talk": "\\rspd=110\\\\vct=135\\ ${target} �B�B�B���߂�Ȃ����B�܂��׋����ŁA�������ł��܂���B�X�^�b�t�܂ł��q�˂��������B"}',sysdatetime());

GO
