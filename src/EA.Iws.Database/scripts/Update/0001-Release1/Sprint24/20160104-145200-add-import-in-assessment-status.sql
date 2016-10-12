INSERT INTO [Lookup].[ImportNotificationStatus]
(
	[Id], 
	[Description]
)
VALUES	(6, 'In assessment');

GO

UPDATE [Lookup].[ImportNotificationStatus]
SET [Description] = 'Notification received'
WHERE [Description] = 'NotificationReceived';

GO

ALTER TABLE [ImportNotification].[NotificationDates]
ADD [AssessmentStartedDate] DATETIMEOFFSET NULL;

GO

ALTER TABLE [ImportNotification].[NotificationDates]
ADD [NameOfOfficer] NVARCHAR(256) NULL;

GO

UPDATE [ImportNotification].[NotificationAssessment]
SET [Status] = 4
WHERE [NotificationApplicationId] IN (SELECT [ImportNotificationId] FROM [ImportNotification].[Exporter])
AND Status = 2;

GO