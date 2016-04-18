CREATE TABLE [ImportNotification].[Withdrawn]
(
	[Id] UNIQUEIDENTIFIER CONSTRAINT PK_ImportNotification_Withdrawn PRIMARY KEY NOT NULL,
	[Date] DATE NOT NULL,
	[Reasons] NVARCHAR(4000) NOT NULL,
	[NotificationId] UNIQUEIDENTIFIER CONSTRAINT FK_Withdrawn_ImportNotification FOREIGN KEY REFERENCES [ImportNotification].[Notification]([Id]),
	[RowVersion] ROWVERSION NOT NULL
);
GO

CREATE TABLE [ImportNotification].[Objection]
(
	[Id] UNIQUEIDENTIFIER CONSTRAINT PK_ImportNotification_Objection PRIMARY KEY NOT NULL,
	[Date] DATE NOT NULL,
	[Reasons] NVARCHAR(4000) NOT NULL,
	[NotificationId] UNIQUEIDENTIFIER CONSTRAINT FK_Objection_ImportNotification FOREIGN KEY REFERENCES [ImportNotification].[Notification]([Id]),
	[RowVersion] ROWVERSION NOT NULL
);
GO

INSERT INTO [ImportNotification].[Objection]
(
	[Id],
	[NotificationId],
	[Reasons],
	[Date]
)
SELECT	(SELECT Cast(Cast(Newid() AS BINARY(10)) + Cast(Getdate() AS BINARY(6)) AS UNIQUEIDENTIFIER)),
		[NA].[NotificationApplicationId],
		COALESCE([D].[ObjectedReason], 'No specific reason provided'),
		[D].[ObjectedDate]

FROM [ImportNotification].[NotificationDates] AS D

INNER JOIN [ImportNotification].[NotificationAssessment] AS NA
ON NA.[Id] = D.[NotificationAssessmentId]

WHERE D.ObjectedDate IS NOT NULL;
GO

ALTER TABLE [ImportNotification].[NotificationDates]
DROP COLUMN [ObjectedDate];
GO

ALTER TABLE [ImportNotification].[NotificationDates]
DROP COLUMN [ObjectedReason];
GO