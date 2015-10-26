GO

DELETE FROM [Lookup].[EntryOrExitPoint]
WHERE [Name] = 'Kiel Canal' AND [CountryId] = (SELECT [Id] FROM [Lookup].[Country] WHERE [Name]='Germany');
GO