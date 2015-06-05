

GO
PRINT N'Altering [Business].[Address]...';


GO
ALTER TABLE [Business].[Address] ALTER COLUMN PostalCode nvarchar(64) NULL;

GO
ALTER TABLE [Business].[Organisation] ALTER COLUMN PostalCode nvarchar(64) NULL;

GO
ALTER TABLE [Business].[Carrier] ALTER COLUMN PostalCode nvarchar(64) NULL;

GO
ALTER TABLE [Business].[Exporter] ALTER COLUMN PostalCode nvarchar(64) NULL;

GO
ALTER TABLE [Business].[Facility] ALTER COLUMN PostalCode nvarchar(64) NULL;

GO
ALTER TABLE [Business].[Producer] ALTER COLUMN PostalCode nvarchar(64) NULL;

GO
ALTER TABLE [Business].[Importer] ALTER COLUMN PostalCode nvarchar(64) NULL;


GO
PRINT N'Update complete.';


GO
