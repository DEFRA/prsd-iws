INSERT INTO [Lookup].[ImportNotificationStatus]
(
	[Id], 
	[Description]
)
VALUES	(7, 'Ready to acknowledge'), 
		(8, 'Decision required by');

GO

ALTER TABLE [ImportNotification].[NotificationDates]
ADD [NotificationCompletedDate] DATETIMEOFFSET NULL;

GO

ALTER TABLE [ImportNotification].[NotificationDates]
ADD [AcknowledgedDate] DATETIMEOFFSET NULL;

GO