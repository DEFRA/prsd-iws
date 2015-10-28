GO

IF EXISTS (SELECT 1 FROM [Lookup].[EntryOrExitPoint] WHERE [Name] = 'Brünsbuttel' AND [CountryId] = (SELECT [Id] FROM [Lookup].[Country] WHERE [Name]='Germany'))
UPDATE [Lookup].[EntryOrExitPoint]
SET [Name] = 'Brunsbüttel'
WHERE [Name] = 'Brünsbuttel' AND [CountryId] = (SELECT [Id] FROM [Lookup].[Country] WHERE [Name]='Germany');
GO