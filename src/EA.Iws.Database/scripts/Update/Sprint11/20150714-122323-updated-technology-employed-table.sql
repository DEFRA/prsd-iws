SET ANSI_WARNINGS  OFF;

GO
ALTER TABLE [Notification].[TechnologyEmployed] ALTER COLUMN Details [nvarchar](70) NULL

GO
ALTER TABLE [Notification].[TechnologyEmployed]  ADD [FurtherDetails] [nvarchar](max) NULL

SET ANSI_WARNINGS ON;

PRINT N'Update complete.';
GO
