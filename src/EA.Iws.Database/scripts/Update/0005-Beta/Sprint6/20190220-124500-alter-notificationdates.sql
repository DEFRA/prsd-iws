ALTER TABLE [ImportNotification].[NotificationDates]
ADD [DecisionRequiredByDate] DATE NULL;
GO

ALTER TABLE [Notification].[NotificationDates]
ADD [DecisionRequiredByDate] DATE NULL;
GO