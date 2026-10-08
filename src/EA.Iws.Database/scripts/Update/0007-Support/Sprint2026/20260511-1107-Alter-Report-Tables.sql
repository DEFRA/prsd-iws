ALTER TABLE [Reports].[ProducerCache] ADD [RegistrationNumber] NVARCHAR(64) NULL;
GO

ALTER TABLE [Reports].[FreedomOfInformationCache] ADD [RegistrationNumber] NVARCHAR(64) NULL;
GO

ALTER TABLE [Reports].[FreedomOfInformationCache] ADD [ConsentWithdrawnDate] DATE NULL;
GO

ALTER TABLE [Reports].[FreedomOfInformationCache] ADD [CustomsCode] NVARCHAR(MAX) NULL;
GO

ALTER TABLE [Reports].[ShipmentsCache] ADD [RegistrationNumber] NVARCHAR(64) NULL;
GO

ALTER TABLE [Reports].[ShipmentsCache] ADD [CustomsCode] NVARCHAR(MAX) NULL;
GO