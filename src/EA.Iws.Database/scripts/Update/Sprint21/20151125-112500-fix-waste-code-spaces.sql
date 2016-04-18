-- Replace non breaking spaces with standard spaces
UPDATE [Lookup].[WasteCode]
SET [Code] = REPLACE([Code], NCHAR(0x00A0), ' '),
	[Description] = REPLACE([Description], NCHAR(0x00A0), ' ')