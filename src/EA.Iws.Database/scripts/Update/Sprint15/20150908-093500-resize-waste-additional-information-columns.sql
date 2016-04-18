ALTER TABLE [Notification].[WasteAdditionalInformation]
ALTER COLUMN MaxConcentration DECIMAL(7, 2) NOT NULL;

ALTER TABLE [Notification].[WasteAdditionalInformation]
ALTER COLUMN MinConcentration DECIMAL(7, 2) NOT NULL;
GO