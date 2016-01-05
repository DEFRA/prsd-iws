INSERT INTO [Lookup].[ImportNotificationStatus]
(
	[Id], 
	[Description]
)
VALUES	(9, 'Consented');

GO

ALTER TABLE [ImportNotification].[NotificationDates]
ADD [ConsentedDate] DATETIMEOFFSET NULL;

GO

CREATE TABLE [ImportNotification].[Consent]
(
	[Id] UNIQUEIDENTIFIER CONSTRAINT PK_Consent PRIMARY KEY NOT NULL,
	[From] DATETIMEOFFSET NOT NULL,
	[To] DATEOFFSET NOT NULL,
	[Conditions] NVARCHAR(4000) NULL,
	[UserId] UNIQUEIDENTIFIER NOT NULL,
	[NotificationId] UNIQUEIDENTIFIER CONSTRAINT FK_ImportConsent_ImportNotification FOREIGN KEY REFERENCES [ImportNotification].[Notification]([Id]) NOT NULL,
	[RowVersion] ROWVERSION NOT NULL
);
GO