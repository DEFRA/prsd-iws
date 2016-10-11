ALTER TABLE [ImportNotification].[Facility]
ALTER COLUMN [RegistrationNumber] NVARCHAR(100) NULL;
GO

ALTER TABLE [ImportNotification].[Importer]
ALTER COLUMN [RegistrationNumber] NVARCHAR(100) NULL;
GO