DROP TABLE [Business].[Address]
GO

DROP TABLE [Business].[Contact]
GO

ALTER TABLE [Business].[Carrier]
DROP COLUMN [CountryId]
GO

ALTER TABLE [Business].[Exporter]
DROP COLUMN [CountryId]
GO

ALTER TABLE [Business].[Facility]
DROP COLUMN [CountryId]
GO

ALTER TABLE [Business].[Importer]
DROP COLUMN [CountryId]
GO