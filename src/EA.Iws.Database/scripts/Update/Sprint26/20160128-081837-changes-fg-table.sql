GO

ALTER TABLE [Notification].[FinancialGuarantee]
DROP COLUMN [AmountOfCoverProvided];

GO

EXEC sp_rename 'Notification.FinancialGuarantee.ApprovedFrom', 'ValidFrom', 'COLUMN';

GO

EXEC sp_rename 'Notification.FinancialGuarantee.ApprovedTo', 'ValidTo', 'COLUMN';

GO

EXEC sp_rename 'Notification.FinancialGuarantee.BlanketBondReference', 'ReferenceNumber', 'COLUMN';

GO

ALTER TABLE [Notification].[FinancialGuarantee]
ADD IsBlanketBond bit;

GO
