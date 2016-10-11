INSERT INTO [Lookup].[ImportNotificationStatus]
(
	[Id], 
	[Description]
)
VALUES	(3, 'Submitted'),
		(4, 'Awaiting payment'),
		(5, 'Awaiting assessment');

GO

UPDATE [ImportNotification].[NotificationAssessment]
SET [Status] = 3
WHERE [NotificationApplicationId] IN (SELECT [ImportNotificationId] FROM [ImportNotification].[Producer]);

GO