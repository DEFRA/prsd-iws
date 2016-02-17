CREATE TABLE [ImportNotification].[Consultation]
(
	[Id] UNIQUEIDENTIFIER 
		CONSTRAINT PK_ImportNotification_Consultation 
		PRIMARY KEY NOT NULL,
	[NotificationId] UNIQUEIDENTIFIER 
		CONSTRAINT FK_ImportNotification_Consultation_ImportNotification_Notification
		FOREIGN KEY REFERENCES [ImportNotification].[Notification]([Id]) NOT NULL,
	[LocalAreaId] UNIQUEIDENTIFIER 
		CONSTRAINT FK_ImportNotification_Consultation_Lookup_LocalArea 
		FOREIGN KEY REFERENCES [Lookup].[LocalArea]([Id]) NULL,
	[ReceivedDate] DATE NULL,
	[RowVersion] ROWVERSION NOT NULL
);
GO

INSERT INTO [ImportNotification].[Consultation]
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
	[ImportNotification].[NotificationAssessment] NA
WHERE
	[LocalAreaId] IS NOT NULL