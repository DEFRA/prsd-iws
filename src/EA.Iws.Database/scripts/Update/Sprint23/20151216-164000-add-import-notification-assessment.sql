CREATE TABLE [Lookup].[ImportNotificationStatus](
	[Id]				INT NOT NULL,
	[Description]		NVARCHAR(64) NOT NULL,
	CONSTRAINT [PK_ImportNotificationStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

INSERT INTO [Lookup].[ImportNotificationStatus]([Id], [Description])
VALUES (1, 'New');
GO

CREATE TABLE [ImportNotification].[NotificationAssessment] (
    [Id]								UNIQUEIDENTIFIER	NOT NULL,
    [NotificationApplicationId]			UNIQUEIDENTIFIER	NOT NULL,
	[Status]							INT					NOT NULL,
    [RowVersion]						ROWVERSION			NOT NULL,
    CONSTRAINT [PK_ImportNotificationAssessment] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_ImportNotificationAssessment_Status] FOREIGN KEY ([Status]) REFERENCES [Lookup].[ImportNotificationStatus]([Id])
);
GO

CREATE TABLE [ImportNotification].[NotificationDates] (
    [Id]								UNIQUEIDENTIFIER	NOT NULL CONSTRAINT [PK_ImportNotificationDates] PRIMARY KEY,
    [NotificationAssessmentId]			UNIQUEIDENTIFIER	NOT NULL CONSTRAINT [FK_ImportNotificationDates_ImportNotificationAssessment] FOREIGN KEY REFERENCES [ImportNotification].[NotificationAssessment] ([Id]),
    [RowVersion]						ROWVERSION			NOT NULL,
	[NotificationReceivedDate]			DATETIMEOFFSET		NULL,
    [PaymentReceivedDate]				DATETIMEOFFSET		NULL,
	[WithdrawnDate]						DATETIMEOFFSET		NULL  
);
GO

CREATE TABLE [ImportNotification].[NotificationStatusChange]
(
	[Id]								UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_ImportNotificationStatusChange PRIMARY KEY,
	[NotificationAssessmentId]			UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_ImportNotificationStatusChange_NotificationAssessment FOREIGN KEY REFERENCES [ImportNotification].[NotificationAssessment]([Id]),
	[PreviousStatus]					INT NOT NULL,
	[NewStatus]						INT NOT NULL,
	[UserId]							NVARCHAR(128) NOT NULL CONSTRAINT FK_ImportNotificationStatusChange_User FOREIGN KEY REFERENCES [Identity].[AspNetUsers]([Id]),
	[ChangeDate]						DATETIMEOFFSET NOT NULL 
	CONSTRAINT DF_ImportNotificationStatusChange_ChangeDate DEFAULT GETDATE(),
	[RowVersion]						ROWVERSION NOT NULL
);
GO

INSERT INTO		[ImportNotification].[NotificationAssessment]([Id], [NotificationApplicationId], [Status])
SELECT			(SELECT Cast(Cast(Newid() AS BINARY(10))
                           + Cast(Getdate() AS BINARY(6)) AS UNIQUEIDENTIFIER)), Id, 1 
FROM			[ImportNotification].[Notification]
WHERE			Id NOT IN (SELECT NotificationApplicationId FROM [ImportNotification].[NotificationAssessment]);

GO

INSERT INTO		[ImportNotification].[NotificationDates]([Id], [NotificationAssessmentId])
SELECT			(SELECT Cast(Cast(Newid() AS BINARY(10))
                           + Cast(Getdate() AS BINARY(6)) AS UNIQUEIDENTIFIER)), 
						   Id
FROM			[ImportNotification].[NotificationAssessment]
WHERE			Id NOT IN (SELECT NotificationAssessmentId FROM [ImportNotification].[NotificationDates]);

GO