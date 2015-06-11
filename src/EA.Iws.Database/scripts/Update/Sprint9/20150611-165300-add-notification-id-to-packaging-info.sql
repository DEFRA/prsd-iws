-----------
-- Add new foreign key
-----------
ALTER TABLE [Business].[PackagingInfo]
ADD [NotificationId] UNIQUEIDENTIFIER NULL CONSTRAINT FK_PackagingInfo_Notification 
FOREIGN KEY ([NotificationId]) REFERENCES [Notification].[Notification]([Id])
GO

-----------
-- Migrate data
-----------
DECLARE @notificationId uniqueidentifier
DECLARE @entityId uniqueidentifier
DECLARE @cursor CURSOR

SET @cursor = CURSOR FOR
SELECT
P.Id,
S.NotificationId
FROM [Business].[PackagingInfo] P
INNER JOIN [Business].[ShipmentInfo] S on P.[ShipmentInfoId] = S.[Id]

OPEN @cursor
FETCH NEXT
FROM @cursor INTO @entityId, @notificationId
WHILE @@FETCH_STATUS = 0
BEGIN
UPDATE [Business].[PackagingInfo]
SET [NotificationId] = @notificationId
WHERE Id = @entityId
FETCH NEXT
FROM @cursor INTO @entityId, @notificationId

END

CLOSE @cursor
DEALLOCATE @cursor

GO

-----------
-- Drop unused column
-----------
ALTER TABLE [Business].[PackagingInfo] DROP FK_PackagingInfo_ShipmentInfo
ALTER TABLE [Business].[PackagingInfo] DROP COLUMN [ShipmentInfoId]
GO

-----------
-- Update constraints to not null
-----------
ALTER TABLE [Business].[PackagingInfo] ALTER COLUMN [NotificationId] UNIQUEIDENTIFIER NOT NULL
GO