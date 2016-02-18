CREATE TABLE [Notification].[Consultation]
(
	[Id] UNIQUEIDENTIFIER 
		CONSTRAINT PK_Notification_Consultation 
		PRIMARY KEY NOT NULL,
	[NotificationId] UNIQUEIDENTIFIER 
		CONSTRAINT FK_Notification_Consultation_Notification_Notification
		FOREIGN KEY REFERENCES [Notification].[Notification]([Id]) NOT NULL,
	[LocalAreaId] UNIQUEIDENTIFIER 
		CONSTRAINT FK_Notification_Consultation_Lookup_LocalArea 
		FOREIGN KEY REFERENCES [Lookup].[LocalArea]([Id]) NULL,
	[ReceivedDate] DATE NULL,
	[RowVersion] ROWVERSION NOT NULL
);
GO

INSERT INTO [Notification].[Consultation]
(
	[Id],
	[NotificationId],
	[LocalAreaId]
)

SELECT
	(SELECT CAST(CAST(NEWID() AS BINARY(10)) + CAST(GETDATE() AS BINARY(6)) AS UNIQUEIDENTIFIER)),
	NA.NotificationApplicationId,
	NA.LocalAreaId
FROM
	[Notification].[NotificationAssessment] NA
WHERE
	[LocalAreaId] IS NOT NULL;
GO

ALTER TABLE [Notification].[NotificationAssessment]
DROP COLUMN [LocalAreaId];
GO