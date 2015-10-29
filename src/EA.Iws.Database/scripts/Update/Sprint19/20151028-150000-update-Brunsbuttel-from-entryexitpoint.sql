UPDATE [Lookup].[EntryOrExitPoint]
SET [Name] = 'Brunsbüttel'
WHERE [Name] = 'Brünsbuttel' AND [CountryId] = (SELECT [Id] FROM [Lookup].[Country] WHERE [Name]='Germany');