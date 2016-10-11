ALTER TABLE [Person].[AddressBookRecord]
ALTER COLUMN [FirstName] NVARCHAR(2048)
GO

EXEC sp_rename '[Person].[AddressBookRecord].[FirstName]', 'FullName', 'COLUMN';
GO

UPDATE [Person].[AddressBookRecord]
SET [FullName] = [FullName] + ' ' + [LastName]
GO

ALTER TABLE [Person].[AddressBookRecord]
DROP COLUMN [LastName]
GO