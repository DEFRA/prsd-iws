PRINT N'Correcting incorrect refund amount';
-- Export Disposal IsInterim 0 Range 6-20
UPDATE [Lookup].[PricingStructure]
	SET PotentialRefund = '457.00'
	WHERE Id IN ('442C6967-7C77-45F2-95A7-7428E379012A', '7881DFE2-C1E6-45A8-932C-DBC77460DFE9');

PRINT N'Add new SystemSetting as per emails';
INSERT INTO [Lookup].[SystemSettings] (Id, [Value], [Description])
	VALUES (7, '1196', 'EA Export Recovery custom per 100 (or part of) shipments over 1000 additional charge')

PRINT N'Add new SystemSetting as per emails';
INSERT INTO [Lookup].[SystemSettings] (Id, [Value], [Description])
	VALUES (8, '1338', 'EA Export Disposal custom per 100 (or part of) shipments over 1000 additional charge')