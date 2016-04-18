-----------
-- Add new columns
-----------
ALTER TABLE [Business].[WasteCodeInfo]
ADD NotificationId UNIQUEIDENTIFIER NULL CONSTRAINT FK_WasteCodeInfo_Notification FOREIGN KEY REFERENCES [Notification].[Notification]([Id])
GO

-----------
-- Migrate data
-----------
DECLARE @notificationId uniqueidentifier
DECLARE @wasteTypeId uniqueidentifier
DECLARE @cursor CURSOR

SET @cursor = CURSOR FOR
SELECT
wt.NotificationId,
wci.WasteTypeId
FROM [Business].[WasteCodeInfo] wci
INNER JOIN [Business].[WasteType] wt on wci.WasteTypeId = wt.Id

OPEN @cursor
FETCH NEXT
FROM @cursor INTO @notificationId, @wasteTypeId
WHILE @@FETCH_STATUS = 0
BEGIN
UPDATE [Business].[WasteCodeInfo]
SET NotificationId = @notificationId
WHERE WasteTypeId = @wasteTypeId
FETCH NEXT
FROM @cursor INTO @notificationId, @wasteTypeId

END

CLOSE @cursor
DEALLOCATE @cursor

GO

-----------
-- Make column not null
-----------
ALTER TABLE [Business].[WasteCodeInfo]
ALTER COLUMN NotificationId UNIQUEIDENTIFIER NOT NULL
GO

-----------
-- Drop unused columns
-----------
ALTER TABLE [Business].[WasteCodeInfo] DROP CONSTRAINT [FK_WasteCodeInfo_WasteType]
ALTER TABLE [Business].[WasteCodeInfo] DROP COLUMN [WasteTypeId]	
GO