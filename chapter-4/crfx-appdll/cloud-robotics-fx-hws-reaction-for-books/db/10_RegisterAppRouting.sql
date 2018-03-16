--
-- Register app info to RBFX.AppRouting
--
DECLARE @appId AS NVARCHAR(100) = N'OmotenashiPepperSample';
DECLARE @appProcessingId AS NVARCHAR(100) = N'App';

DELETE FROM [RBFX].[AppRouting] 
  WHERE AppId = @appId and AppProcessingId = @appProcessingId;

INSERT INTO [RBFX].[AppRouting]
(
  AppId,AppProcessingId,BlobContainer,[FileName],ClassName,
  [Status],DevMode,DevLocalDir,[Description],Registered_DateTime
)
VALUES
(
  @appId,
  @appProcessingId,
  'appdll',
  'OmotenashiPepperSample.dll',
  'OmotenashiPepperSample.App',
  'Active',
  'False',
  '',
  '',
  sysdatetime()
);

SELECT * FROM [RBFX].[AppRouting]
  WHERE AppId = @appId and AppProcessingId = @appProcessingId;
