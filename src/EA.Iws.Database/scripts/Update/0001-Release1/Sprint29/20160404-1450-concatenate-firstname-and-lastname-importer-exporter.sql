-- Update the Importer Table

ALTER TABLE [Notification].[Importer]
ALTER COLUMN [FirstName] NVARCHAR(2048)
GO

EXEC sp_rename '[Notification].[Importer].[FirstName]', 'FullName', 'COLUMN';
GO

UPDATE [Notification].[Importer]
SET [FullName] = [FullName] + ' ' + [LastName]
GO

ALTER TABLE [Notification].[Importer]
DROP COLUMN [LastName]
GO

-- Update the Exporter Table

ALTER TABLE [Notification].[Exporter]
ALTER COLUMN [FirstName] NVARCHAR(2048)
GO

EXEC sp_rename '[Notification].[Exporter].[FirstName]', 'FullName', 'COLUMN';
GO

UPDATE [Notification].[Exporter]
SET [FullName] = [FullName] + ' ' + [LastName]
GO

ALTER TABLE [Notification].[Exporter]
DROP COLUMN [LastName]
GO