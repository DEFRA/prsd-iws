-----------
-- Add new columns
-----------
ALTER TABLE [Notification].[Notification]
ADD WasteGenerationProcess [nvarchar](1024) NULL
GO

ALTER TABLE [Notification].[Notification] 
ADD IsWasteGenerationProcessAttached [bit] NULL
GO

-----------
-- Migrate data
-----------
DECLARE @notificationId uniqueidentifier
DECLARE @isAttached bit
DECLARE @process nvarchar(1024)
DECLARE @cursor CURSOR

SET @cursor = CURSOR FOR
SELECT
NotificationId,
IsDocumentAttached,
WasteGenerationProcess
FROM [Business].[WasteType]

OPEN @cursor
FETCH NEXT
FROM @cursor INTO @notificationId, @isAttached, @process
WHILE @@FETCH_STATUS = 0
BEGIN
UPDATE [Notification].[Notification]
SET IsWasteGenerationProcessAttached = @isAttached,
WasteGenerationProcess = @process
WHERE Id = @notificationId
FETCH NEXT
FROM @cursor INTO @notificationId, @isAttached, @process

END

CLOSE @cursor
DEALLOCATE @cursor

GO

-----------
-- Drop unused columns
-----------
ALTER TABLE [Business].[WasteType] DROP CONSTRAINT [DF_WasteType_IsDocumentAttached]
ALTER TABLE [Business].[WasteType] DROP COLUMN [WasteGenerationProcess]	
ALTER TABLE [Business].[WasteType] DROP COLUMN [IsDocumentAttached]
GO