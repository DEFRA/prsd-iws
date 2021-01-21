GO
PRINT N'ALTER [Lookup].[WasteCode]...';

IF EXISTS 
(
    SELECT * 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE table_name = 'Lookup.WasteCode'
    AND column_name = 'Active'
)
	BEGIN
		ALTER TABLE [Lookup].[WasteCode]
		ADD [Active] bit NOT NULL
		DEFAULT 1;
	END;
	
GO
PRINT N'ALTER complete...';