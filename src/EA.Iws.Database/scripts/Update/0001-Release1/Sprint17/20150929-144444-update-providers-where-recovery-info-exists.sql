UPDATE 
	N
SET 
	N.IsRecoveryPercentageDataProvidedByImporter = 0
FROM 
	[Notification].[Notification] N
	INNER JOIN [Notification].[RecoveryInfo] RI ON RI.NotificationId = N.Id
WHERE 
	IsRecoveryPercentageDataProvidedByImporter IS NULL
	AND NotificationType = 1;