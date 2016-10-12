GO

/*	Chemical Composition - Wood Type - To remove any existing data of food as constituent 
	Reference: PBI# 29764:Change listed contituents on chem composition screen for Wood notifications */
IF EXISTS (SELECT 1 FROM [Business].[WasteComposition] WHERE [ChemicalCompositionType]=3 AND [Constituent]='Food')
BEGIN
	DELETE [Business].[WasteComposition] WHERE [ChemicalCompositionType]=3 AND [Constituent]='Food'
END
GO