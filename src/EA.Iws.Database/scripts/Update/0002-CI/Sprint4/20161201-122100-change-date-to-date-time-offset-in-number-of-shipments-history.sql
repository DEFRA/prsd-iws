ALTER TABLE [ImportNotification].[NumberOfShipmentsHistory]
ALTER COLUMN [DateChanged] DATETIMEOFFSET(0) NOT NULL;

ALTER TABLE [Notification].[NumberOfShipmentsHistory]
ALTER COLUMN [DateChanged] DATETIMEOFFSET(0) NOT NULL;