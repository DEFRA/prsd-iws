EXEC sp_rename '[Notification].[Consent].[ValidFrom]', 'From', 'COLUMN';
GO

EXEC sp_rename '[Notification].[Consent].[ValidTo]', 'To', 'COLUMN';
GO