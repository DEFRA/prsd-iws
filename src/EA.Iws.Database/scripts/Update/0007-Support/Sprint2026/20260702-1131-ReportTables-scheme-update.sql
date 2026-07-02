IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'Reports' AND TABLE_NAME = 'ProducerCache' AND COLUMN_NAME = 'RegistrationNumber')
BEGIN
    ALTER TABLE [Reports].[ProducerCache] DROP COLUMN [RegistrationNumber];
END;
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'Reports' AND TABLE_NAME = 'ProducerCache' AND COLUMN_NAME = 'ExporterRegistrationNumber')
BEGIN
    ALTER TABLE [Reports].[ProducerCache] ADD [ExporterRegistrationNumber] NVARCHAR(64) NULL;
END;
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'Reports' AND TABLE_NAME = 'ProducerCache' AND COLUMN_NAME = 'ImporterRegistrationNumber')
BEGIN
    ALTER TABLE [Reports].[ProducerCache] ADD [ImporterRegistrationNumber] NVARCHAR(64) NULL;
END;
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'Reports' AND TABLE_NAME = 'ProducerCache' AND COLUMN_NAME = 'FacilityRegistrationNumber')
BEGIN
    ALTER TABLE [Reports].[ProducerCache] ADD [FacilityRegistrationNumber] NVARCHAR(64) NULL;
END;
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'Reports' AND TABLE_NAME = 'ProducerCache' AND COLUMN_NAME = 'ProducerRegistrationNumber')
BEGIN
    ALTER TABLE [Reports].[ProducerCache] ADD [ProducerRegistrationNumber] NVARCHAR(64) NULL;
END;
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'Reports' AND TABLE_NAME = 'FreedomOfInformationCache' AND COLUMN_NAME = 'RegistrationNumber')
BEGIN
    ALTER TABLE [Reports].[FreedomOfInformationCache] DROP COLUMN [RegistrationNumber];
END;
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'Reports' AND TABLE_NAME = 'FreedomOfInformationCache' AND COLUMN_NAME = 'ExporterRegistrationNumber')
BEGIN
    ALTER TABLE [Reports].[FreedomOfInformationCache] ADD [ExporterRegistrationNumber] NVARCHAR(64) NULL;
END;
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'Reports' AND TABLE_NAME = 'FreedomOfInformationCache' AND COLUMN_NAME = 'ImporterRegistrationNumber')
BEGIN
    ALTER TABLE [Reports].[FreedomOfInformationCache] ADD [ImporterRegistrationNumber] NVARCHAR(64) NULL;
END;
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'Reports' AND TABLE_NAME = 'FreedomOfInformationCache' AND COLUMN_NAME = 'FacilityRegistrationNumber')
BEGIN
    ALTER TABLE [Reports].[FreedomOfInformationCache] ADD [FacilityRegistrationNumber] NVARCHAR(64) NULL;
END;
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'Reports' AND TABLE_NAME = 'FreedomOfInformationCache' AND COLUMN_NAME = 'ProducerRegistrationNumber')
BEGIN
    ALTER TABLE [Reports].[FreedomOfInformationCache] ADD [ProducerRegistrationNumber] NVARCHAR(64) NULL;
END;
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'Reports' AND TABLE_NAME = 'ShipmentsCache' AND COLUMN_NAME = 'RegistrationNumber')
BEGIN
    ALTER TABLE [Reports].[ShipmentsCache] DROP COLUMN [RegistrationNumber];
END;

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'Reports' AND TABLE_NAME = 'ShipmentsCache' AND COLUMN_NAME = 'ExporterRegistrationNumber')
BEGIN
    ALTER TABLE [Reports].[ShipmentsCache] ADD [ExporterRegistrationNumber] NVARCHAR(64) NULL;
END;
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'Reports' AND TABLE_NAME = 'ShipmentsCache' AND COLUMN_NAME = 'ImporterRegistrationNumber')
BEGIN
    ALTER TABLE [Reports].[ShipmentsCache] ADD [ImporterRegistrationNumber] NVARCHAR(64) NULL;
END;
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'Reports' AND TABLE_NAME = 'ShipmentsCache' AND COLUMN_NAME = 'FacilityRegistrationNumber')
BEGIN
    ALTER TABLE [Reports].[ShipmentsCache] ADD [FacilityRegistrationNumber] NVARCHAR(64) NULL;
END;
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'Reports' AND TABLE_NAME = 'ShipmentsCache' AND COLUMN_NAME = 'ProducerRegistrationNumber')
BEGIN
    ALTER TABLE [Reports].[ShipmentsCache] ADD [ProducerRegistrationNumber] NVARCHAR(64) NULL;
END;
GO