ALTER TABLE [Notification].[RecoveryInfo] ADD [PercentageRecoverable] DECIMAL(18,2) NULL;
GO

DELETE FROM 
	[Notification].[RecoveryInfo]
FROM 
	[Notification].[RecoveryInfo] RI
	INNER JOIN [Notification].[Notification] N 
		ON RI.[NotificationId] = N.[Id]
WHERE 
	N.[IsRecoveryPercentageDataProvidedByImporter] = 1;
GO

UPDATE
	R
SET
	R.[PercentageRecoverable] = N.[PercentageRecoverable]
FROM
	[Notification].[RecoveryInfo] R
	INNER JOIN [Notification].[Notification] N
		ON R.NotificationId = N.Id;
GO

ALTER TABLE [Notification].[RecoveryInfo] ALTER COLUMN [PercentageRecoverable] DECIMAL(18,2) NOT NULL;
GO

ALTER TABLE [Notification].[Notification] DROP COLUMN [PercentageRecoverable];
GO