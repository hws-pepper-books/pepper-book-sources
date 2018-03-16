--
-- Register app info to RBFX.AppMaster
--
DECLARE @appId AS NVARCHAR(100);
DECLARE @storageAccount AS NVARCHAR(256);
DECLARE @storageKey AS NVARCHAR(1000);
DECLARE @appInfo AS NVARCHAR(4000);
DECLARE @appInfoDevice AS NVARCHAR(4000);
DECLARE @passPhrase AS NVARCHAR(100);
SET @appId = N'SynAppsFace';
SET @storageAccount = N'youazurestorageaccountid';
SET @storageKey = N'youazurestorageaccountkey';
SET @appInfo = N'{';
SET @appInfo += N'"StorageAccount": "youazurestorageaccountid"';
SET @appInfo += N',"StorageKey": "youazurestorageaccountkey"';
SET @appInfo += N',"StorageContainer": "cogapifiles"';
SET @appInfo += N',"FaceApiKey": "yourfaceapikey"';
SET @appInfo += N',"FacialHairConfidence": "0.6"';
SET @appInfo += N'}';
SET @appInfoDevice = N'{ "InitialData": [] }'
SET @passPhrase = N'yourencryptkeyphrase'

DELETE FROM [RBFX].[AppMaster] WHERE AppId = @appId;

INSERT INTO [RBFX].[AppMaster]
(
  AppId,StorageAccount,StorageKeyEnc,AppInfoEnc,AppInfoDeviceEnc,[Status],[Description],Registered_DateTime
)
VALUES
(
  @appId,
  @storageAccount,
  EncryptByPassPhrase(@passPhrase, @storageKey, 1, CONVERT(varbinary, @appId)),
  EncryptByPassPhrase(@passPhrase, @appInfo, 1, CONVERT(varbinary, @appId)),
  EncryptByPassPhrase(@passPhrase, @appInfoDevice, 1, CONVERT(varbinary, @appId)),
  'Active',
  '',
  sysdatetime()
);


SELECT [SeqId]
      ,[AppId]
      ,[StorageAccount]
      ,[StorageKeyEnc]
      ,CONVERT(NVARCHAR(1000),DecryptByPassphrase(@passPhrase,StorageKeyEnc,1,CONVERT(varbinary,AppId))) AS StorageKey
      ,[AppInfoEnc]
      ,CONVERT(NVARCHAR(4000),DecryptByPassphrase(@passPhrase,AppInfoEnc,1,CONVERT(varbinary,AppId))) AS AppInfo
      ,[AppInfoDeviceEnc]
      ,CONVERT(NVARCHAR(4000),DecryptByPassphrase(@passPhrase,AppInfoDeviceEnc,1,CONVERT(varbinary,AppId))) AS AppInfoDevice
      ,[Status]
      ,[Description]
      ,[Registered_DateTime]
  FROM [RBFX].[AppMaster]
  WHERE AppId = @appId;

