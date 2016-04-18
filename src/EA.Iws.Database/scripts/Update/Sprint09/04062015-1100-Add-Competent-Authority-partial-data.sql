INSERT INTO [Lookup].[CompetentAuthority] ([Name], [Abbreviation], [Code], [IsSystemUser], [CountryId])
VALUES ('Environment Agency', 'EA', 'GB01', 1, (SELECT Id From [Lookup].[Country] WHERE Name = 'United Kingdom')),
('Thuringen', null, 'DE0023', 0, (SELECT Id From [Lookup].[Country] WHERE Name = 'Germany')),
('Hamburg', null, 'DE009', 0, (SELECT Id From [Lookup].[Country] WHERE Name = 'Germany')),
('Schleswig-Holstein', null, 'DE0025', 0, (SELECT Id From [Lookup].[Country] WHERE Name = 'Germany'));
GO

INSERT INTO [Lookup].[EntryOrExitPoint] ([Name], [CountryId])
VALUES ('Aachen', (SELECT Id From [Lookup].[Country] WHERE Name = 'Germany')),
('Berlin', (SELECT Id From [Lookup].[Country] WHERE Name = 'Germany')),
('Kiel Canal', (SELECT Id From [Lookup].[Country] WHERE Name = 'Germany')),
('Hamburg', (SELECT Id From [Lookup].[Country] WHERE Name = 'Germany'));
GO