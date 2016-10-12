ALTER TABLE [Notification].[FinancialGuarantee]
ADD [ReleasedDate] DATETIME NULL;
GO

UPDATE [Notification].[FinancialGuarantee]
SET [ReleasedDate] = [DecisionDate]
WHERE [Status] = 6;
GO