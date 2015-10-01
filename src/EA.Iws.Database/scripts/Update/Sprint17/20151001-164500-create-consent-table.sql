CREATE TABLE [Notification].[Consent]
(
	[Id] UNIQUEIDENTIFIER CONSTRAINT PK_Consent PRIMARY KEY NOT NULL,
	[ValidFrom] DATE NOT NULL,
	[ValidTo] DATE NOT NULL,
	[Conditions] NVARCHAR(4000) NOT NULL,
	[UserId] UNIQUEIDENTIFIER NOT NULL,
	[NotificationAssessmentId] UNIQUEIDENTIFIER CONSTRAINT FK_Consent_NotificationAssessment FOREIGN KEY REFERENCES [Notification].[NotificationAssessment]([Id]) NOT NULL,
	[RowVersion] ROWVERSION NOT NULL
);
GO