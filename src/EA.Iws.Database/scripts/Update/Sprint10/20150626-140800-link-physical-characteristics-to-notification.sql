-----------
-- Add new columns
-----------
ALTER TABLE [Business].[PhysicalCharacteristicsInfo]
ADD NotificationId UNIQUEIDENTIFIER NULL CONSTRAINT FK_PhysicalCharacteristicsInfo_Notification FOREIGN KEY REFERENCES [Notification].[Notification]([Id])
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
pci.WasteTypeId
FROM [Business].[PhysicalCharacteristicsInfo] pci
INNER JOIN [Business].[WasteType] wt on pci.WasteTypeId = wt.Id

OPEN @cursor
FETCH NEXT
FROM @cursor INTO @notificationId, @wasteTypeId
WHILE @@FETCH_STATUS = 0
BEGIN
UPDATE [Business].[PhysicalCharacteristicsInfo]
SET NotificationId = @notificationId
WHERE WasteTypeId = @wasteTypeId
FETCH NEXT
FROM @cursor INTO @notificationId, @wasteTypeId

END

CLOSE @cursor
DEALLOCATE @cursor

GO

-----------
-- Drop unused columns
-----------
ALTER TABLE [Business].[PhysicalCharacteristicsInfo] DROP CONSTRAINT [FK_PhysicalCharacteristicsInfo_WasteType]
ALTER TABLE [Business].[PhysicalCharacteristicsInfo] DROP COLUMN [WasteTypeId]	
GO