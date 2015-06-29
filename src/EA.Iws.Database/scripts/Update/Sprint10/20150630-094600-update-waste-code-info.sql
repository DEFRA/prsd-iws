ALTER TABLE [Business].[WasteCodeInfo] DROP COLUMN [OptionalDescription]
GO

EXEC sp_RENAME '[Business].[WasteCodeInfo].[OptionalCode]' , 'CustomCode', 'COLUMN'
GO