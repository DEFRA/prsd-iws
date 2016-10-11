ALTER TABLE [Notification].[FinancialGuarantee]
ADD [DecisionDate] DATETIME2(0) NULL;
GO

ALTER TABLE [Notification].[FinancialGuarantee]
ADD [ApprovedFrom] DATETIME2(0) NULL;
GO

ALTER TABLE [Notification].[FinancialGuarantee]
ADD [ApprovedTo] DATETIME2(0) NULL;
GO

ALTER TABLE [Notification].[FinancialGuarantee]
ADD [RefusalReason] NVARCHAR(2048) NULL;
GO

ALTER TABLE [Notification].[FinancialGuarantee]
ADD [ActiveLoadsPermitted] INT NULL;
GO