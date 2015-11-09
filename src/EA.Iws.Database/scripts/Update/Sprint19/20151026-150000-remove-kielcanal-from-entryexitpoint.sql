GO

DECLARE @Kiel UNIQUEIDENTIFIER = (SELECT [Id] FROM [Lookup].[EntryOrExitPoint] WHERE [Name] = 'Kiel')
DECLARE @KielCanal UNIQUEIDENTIFIER = (SELECT [Id] FROM [Lookup].[EntryOrExitPoint] WHERE [Name] = 'Kiel Canal')

UPDATE [Notification].[StateOfExport]
SET [ExitPointId] = @Kiel
WHERE [ExitPointId] = @KielCanal

UPDATE [Notification].[StateOfImport]
SET [EntryPointId] = @Kiel
WHERE [EntryPointId] = @KielCanal

UPDATE [Notification].[TransitState]
SET [ExitPointId] = @Kiel
WHERE [ExitPointId] = @KielCanal

UPDATE [Notification].[TransitState]
SET [EntryPointId] = @Kiel
WHERE [EntryPointId] = @KielCanal

DELETE FROM [Lookup].[EntryOrExitPoint]
WHERE [Name] = 'Kiel Canal' AND [CountryId] = (SELECT [Id] FROM [Lookup].[Country] WHERE [Name]='Germany');
GO