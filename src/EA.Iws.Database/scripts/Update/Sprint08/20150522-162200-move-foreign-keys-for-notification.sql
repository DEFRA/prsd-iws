-----------
-- Add new foreign keys
-----------
ALTER TABLE [Business].[Exporter]
ADD [NotificationId] UNIQUEIDENTIFIER NULL CONSTRAINT FK_Exporter_Notification REFERENCES [Notification].[Notification] ( Id )
GO

ALTER TABLE [Business].[Producer]
ADD [NotificationId] UNIQUEIDENTIFIER NULL CONSTRAINT FK_Producer_Notification REFERENCES [Notification].[Notification] ( Id )
GO

ALTER TABLE [Business].[Facility]
ADD [NotificationId] UNIQUEIDENTIFIER NULL CONSTRAINT FK_Facility_Notification REFERENCES [Notification].[Notification] ( Id )
GO

ALTER TABLE [Business].[Importer]
ADD [NotificationId] UNIQUEIDENTIFIER NULL CONSTRAINT FK_Importer_Notification REFERENCES [Notification].[Notification] ( Id )
GO


-----------
-- Migrate data
-----------
DECLARE @notificationId uniqueidentifier
DECLARE @entityId uniqueidentifier
DECLARE @cursor CURSOR

-- Exporters
SET @cursor = CURSOR FOR
SELECT
  Id,
  ExporterId
FROM [Notification].[Notification]
WHERE ExporterId IS NOT NULL

OPEN @cursor
FETCH NEXT
FROM @cursor INTO @notificationId, @entityId
WHILE @@FETCH_STATUS = 0
BEGIN
    UPDATE [Business].[Exporter]
    SET NotificationId = @notificationId
    WHERE Id = @entityId
  FETCH NEXT
  FROM @cursor INTO @notificationId, @entityId

END

CLOSE @cursor
DEALLOCATE @cursor

-- Importers
SET @cursor = CURSOR FOR
SELECT
  Id,
  ImporterId
FROM [Notification].[Notification]
WHERE ImporterId IS NOT NULL

OPEN @cursor
FETCH NEXT
FROM @cursor INTO @notificationId, @entityId
WHILE @@FETCH_STATUS = 0
BEGIN
    UPDATE [Business].[Importer]
    SET NotificationId = @notificationId
    WHERE Id = @entityId
  FETCH NEXT
  FROM @cursor INTO @notificationId, @entityId

END

CLOSE @cursor
DEALLOCATE @cursor


-- Facilities
SET @cursor = CURSOR FOR
SELECT
  NotificationId,
  FacilityId
FROM [Notification].[NotificationFacility]

OPEN @cursor
FETCH NEXT
FROM @cursor INTO @notificationId, @entityId
WHILE @@FETCH_STATUS = 0
BEGIN
    UPDATE [Business].[Facility]
    SET NotificationId = @notificationId
    WHERE Id = @entityId
  FETCH NEXT
  FROM @cursor INTO @notificationId, @entityId

END

CLOSE @cursor
DEALLOCATE @cursor


-- Producers
SET @cursor = CURSOR FOR
SELECT
  NotificationId,
  ProducerId
FROM [Notification].[NotificationProducer]

OPEN @cursor
FETCH NEXT
FROM @cursor INTO @notificationId, @entityId
WHILE @@FETCH_STATUS = 0
BEGIN
    UPDATE [Business].[Producer]
    SET NotificationId = @notificationId
    WHERE Id = @entityId
  FETCH NEXT
  FROM @cursor INTO @notificationId, @entityId

END

CLOSE @cursor
DEALLOCATE @cursor

-----------
-- Drop unused tables and columns
-----------
DROP TABLE [Notification].[NotificationFacility]
DROP TABLE [Notification].[NotificationProducer]

ALTER TABLE [Notification].[Notification] DROP FK_Notification_Exporter
ALTER TABLE [Notification].[Notification] DROP FK_Notification_Importer
ALTER TABLE [Notification].[Notification] DROP COLUMN ExporterId
ALTER TABLE [Notification].[Notification] DROP COLUMN ImporterId


-----------
-- Update constraints to not null
-----------
ALTER TABLE [Business].[Exporter] ALTER COLUMN [NotificationId] UNIQUEIDENTIFIER NOT NULL
ALTER TABLE [Business].[Producer] ALTER COLUMN [NotificationId] UNIQUEIDENTIFIER NOT NULL
ALTER TABLE [Business].[Facility] ALTER COLUMN [NotificationId] UNIQUEIDENTIFIER NOT NULL
ALTER TABLE [Business].[Importer] ALTER COLUMN [NotificationId] UNIQUEIDENTIFIER NOT NULL