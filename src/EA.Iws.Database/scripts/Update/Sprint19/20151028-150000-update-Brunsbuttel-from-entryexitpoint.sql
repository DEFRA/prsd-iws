GO

IF EXISTS (SELECT 1 FROM [Lookup].[EntryOrExitPoint] WHERE [Name] = 'Br�nsbuttel' AND [CountryId] = (SELECT [Id] FROM [Lookup].[Country] WHERE [Name]='Germany'))
UPDATE [Lookup].[EntryOrExitPoint]
SET [Name] = 'Brunsb�ttel'
WHERE [Name] = 'Br�nsbuttel' AND [CountryId] = (SELECT [Id] FROM [Lookup].[Country] WHERE [Name]='Germany');
GO