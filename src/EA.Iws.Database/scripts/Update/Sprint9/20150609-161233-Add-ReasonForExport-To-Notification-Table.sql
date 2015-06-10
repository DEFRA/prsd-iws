GO
PRINT N'ALTER [Notification].[Notification]...';

GO
ALTER TABLE [Notification].[Notification]
ADD ReasonForExport [nvarchar](70) NULL


GO
PRINT N'Update complete.';