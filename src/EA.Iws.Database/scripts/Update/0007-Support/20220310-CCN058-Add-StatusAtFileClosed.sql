ALTER TABLE [Notification].[NotificationDates] ADD [StatusAtFileClosed] INT NULL;
ALTER TABLE [ImportNotification].[NotificationDates] ADD [StatusAtFileClosed] INT NULL;
ALTER TABLE [Reports].[FreedomOfInformationCache] ADD [NotificationStatusAtFileClosed] NVARCHAR(64) NULL
GO