IF NOT EXISTS (SELECT 1 FROM [Lookup].[WasteCode] WHERE [CodeType]=5 AND [Code]='H10' AND [Description]='Liberation of toxic gases in contact with air or water')
	UPDATE [Lookup].[WasteCode] 
	SET [Description]='Liberation of toxic gases in contact with air or water'
	WHERE [CodeType]=5 AND [Code]='H10'
GO

IF NOT EXISTS (SELECT 1 FROM [Lookup].[WasteCode] WHERE [CodeType]=5 AND [Code]='H11' AND [Description]='Toxic (delayed or chronic)')
	UPDATE [Lookup].[WasteCode] 
	SET [Description]='Toxic (delayed or chronic)'
	WHERE [CodeType]=5 AND [Code]='H11'
GO

IF NOT EXISTS (SELECT 1 FROM [Lookup].[WasteCode] WHERE [CodeType]=5 AND [Code]='H12' AND [Description]='Toxic (delayed or chronic)')
	UPDATE [Lookup].[WasteCode] 
	SET [Description]='Ecotoxic'
	WHERE [CodeType]=5 AND [Code]='H12'
GO