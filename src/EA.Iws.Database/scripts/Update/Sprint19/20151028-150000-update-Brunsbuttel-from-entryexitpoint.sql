UPDATE [Lookup].[EntryOrExitPoint]
SET [Name] = 'Brunsb�ttel'
WHERE [Name] = 'Br�nsbuttel' AND [CountryId] = (SELECT [Id] FROM [Lookup].[Country] WHERE [Name]='Germany');