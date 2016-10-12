PRINT 'Removing RowVersion column from Country'
ALTER TABLE [Lookup].[Country]
DROP COLUMN RowVersion;
GO

PRINT 'Deleting any rogue competent authority data and inserting correct records'
DECLARE @UK AS UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM [Lookup].[Country] WHERE Name = 'United Kingdom')
DECLARE @FRANCE AS UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM [Lookup].[Country] WHERE Name = 'France')

DECLARE @DELETEDCAS TABLE (Id UNIQUEIDENTIFIER)
INSERT INTO @DELETEDCAS (Id)(
	SELECT ID FROM [Lookup].[CompetentAuthority]
	WHERE CountryId IN (@UK, @FRANCE)
)

DELETE FROM [Notification].[StateOfExport]
WHERE CompetentAuthorityId IN (SELECT Id FROM @DELETEDCAS)

DELETE FROM [Notification].[TransitState]
WHERE CompetentAuthorityId IN (SELECT Id FROM @DELETEDCAS)

DELETE FROM [Notification].[StateOfImport]
WHERE CompetentAuthorityId IN (SELECT Id FROM @DELETEDCAS)

DELETE FROM [Lookup].[CompetentAuthority]
WHERE Id IN (SELECT Id FROM @DELETEDCAS)

INSERT INTO [Lookup].[CompetentAuthority] (Name, Abbreviation, IsSystemUser, Code, CountryId, Region)
VALUES 
('Environment Agency', 'EA', 1, 'GB01', (@UK), NULL),
('Ministère de l''écologie, du développement durable et de l''énergie', 'DGPR/SPNQE', 0,'FR999', (@FRANCE), NULL),
('Scottish Environmental Protection Agency', 'SEPA', 1, 'GB02', (@UK), 'Scotland')
GO

DELETE FROM [Lookup].[EntryOrExitPoint]
WHERE CountryId IN (SELECT Id FROM [Lookup].[Country] WHERE Name = 'United Kingdom' OR Name = 'France');
GO

INSERT INTO [Lookup].[EntryOrExitPoint] (Id, Name, CountryId)
VALUES
(NEWID(), 'Le Harve', (SELECT Id FROM [Lookup].[Country] WHERE Name = 'France')),
(NEWID(), 'Calais', (SELECT Id FROM [Lookup].[Country] WHERE Name = 'France')),
(NEWID(), 'Hull', (SELECT Id FROM [Lookup].[Country] WHERE Name = 'United Kingdom')),
(NEWID(), 'Dover', (SELECT Id FROM [Lookup].[Country] WHERE Name = 'United Kingdom')),
(NEWID(), 'Portsmouth', (SELECT Id FROM [Lookup].[Country] WHERE Name = 'United Kingdom')),
(NEWID(), 'Southhampton', (SELECT Id FROM [Lookup].[Country] WHERE Name = 'United Kingdom')),
(NEWID(), 'Perth', (SELECT Id FROM [Lookup].[Country] WHERE Name = 'United Kingdom'));
GO

CREATE NONCLUSTERED INDEX IX_EntryOrExitPoint_Country 
ON [Lookup].[EntryOrExitPoint] (CountryId)
GO

CREATE NONCLUSTERED INDEX IX_CompetentAuthority_Country 
ON [Lookup].[CompetentAuthority] (CountryId)
GO