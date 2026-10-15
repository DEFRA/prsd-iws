UPDATE [Lookup].[FinancialGuaranteeStatus]
SET [Description] = 'Decision required'
WHERE [Id] = 3 AND [Description] = 'Application complete';

GO

UPDATE [Lookup].[ImportFinancialGuaranteeStatus]
SET [Description] = 'Decision required'
WHERE [Id] = 3 AND [Description] = 'Application complete';

GO