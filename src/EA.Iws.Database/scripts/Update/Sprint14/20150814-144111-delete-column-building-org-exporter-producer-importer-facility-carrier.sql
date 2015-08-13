
GO
PRINT N'Altering [Business].[Carrier]...';


GO
ALTER TABLE [Business].[Carrier] DROP COLUMN [Building];


GO
PRINT N'Altering [Business].[Exporter]...';


GO
ALTER TABLE [Business].[Exporter] DROP COLUMN [Building];


GO
PRINT N'Altering [Business].[Facility]...';


GO
ALTER TABLE [Business].[Facility] DROP COLUMN [Building];


GO
PRINT N'Altering [Business].[Importer]...';


GO
ALTER TABLE [Business].[Importer] DROP COLUMN [Building];


GO
PRINT N'Altering [Business].[Organisation]...';


GO
ALTER TABLE [Business].[Organisation] DROP COLUMN [Building];


GO
PRINT N'Altering [Business].[Producer]...';


GO
ALTER TABLE [Business].[Producer] DROP COLUMN [Building];


GO
PRINT N'Refreshing [Notification].[uspNotificationProgress]...';


GO
EXECUTE sp_refreshsqlmodule N'[Notification].[uspNotificationProgress]';


GO
PRINT N'Update complete.';


GO
