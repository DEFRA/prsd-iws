UPDATE [Business].[WasteCodeInfo]
SET WasteCodeId = NULL
WHERE CodeType IN (7, 8, 9, 10);
GO