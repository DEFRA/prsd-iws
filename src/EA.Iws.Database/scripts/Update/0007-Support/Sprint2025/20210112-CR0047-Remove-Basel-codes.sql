GO
PRINT N'ALTER [Lookup].[WasteCode]...';

UPDATE [Lookup].[WasteCode]
SET [Active] = 0
WHERE [Code] = 'B3010' OR [Code] = 'GH013'

GO
PRINT N'Update complete.';
