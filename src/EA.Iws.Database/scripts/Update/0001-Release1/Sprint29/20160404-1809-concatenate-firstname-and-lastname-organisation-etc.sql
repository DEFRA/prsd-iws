-- Update the Carrier Table

ALTER TABLE [Notification].[Carrier]
ALTER COLUMN [FirstName] NVARCHAR(2048)
GO

EXEC sp_rename '[Notification].[Carrier].[FirstName]', 'FullName', 'COLUMN';
GO

UPDATE [Notification].[Carrier]
SET [FullName] = [FullName] + ' ' + [LastName]
GO

ALTER TABLE [Notification].[Carrier]
DROP COLUMN [LastName]
GO

-- Update the Organisation Table

ALTER TABLE [Notification].[Organisation]
ALTER COLUMN [FirstName] NVARCHAR(2048)
GO

EXEC sp_rename '[Notification].[Organisation].[FirstName]', 'FullName', 'COLUMN';
GO

UPDATE [Notification].[Organisation]
SET [FullName] = [FullName] + ' ' + [LastName]
GO

ALTER TABLE [Notification].[Organisation]
DROP COLUMN [LastName]
GO

-- Update the Producer Table

ALTER TABLE [Notification].[Producer]
ALTER COLUMN [FirstName] NVARCHAR(2048)
GO

EXEC sp_rename '[Notification].[Producer].[FirstName]', 'FullName', 'COLUMN';
GO

UPDATE [Notification].[Producer]
SET [FullName] = [FullName] + ' ' + [LastName]
GO

ALTER TABLE [Notification].[Producer]
DROP COLUMN [LastName]
GO

-- Update the Facility Table

ALTER TABLE [Notification].[Facility]
ALTER COLUMN [FirstName] NVARCHAR(2048)
GO

EXEC sp_rename '[Notification].[Facility].[FirstName]', 'FullName', 'COLUMN';
GO

UPDATE [Notification].[Facility]
SET [FullName] = [FullName] + ' ' + [LastName]
GO

ALTER TABLE [Notification].[Facility]
DROP COLUMN [LastName]
GO