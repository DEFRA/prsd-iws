DELETE FROM [Notification].[FinancialGuarantee] WHERE [ReceivedDate] IS NULL;

ALTER TABLE [Notification].[FinancialGuarantee]
ALTER COLUMN [ReceivedDate] DATE NOT NULL;