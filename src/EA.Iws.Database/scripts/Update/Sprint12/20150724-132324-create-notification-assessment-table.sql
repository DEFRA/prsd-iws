GO
PRINT N'Creating [Notification].[NotificationAssessment]...';


GO
CREATE TABLE [Notification].[NotificationAssessment] (
    [Id]								UNIQUEIDENTIFIER NOT NULL,
    [NotificationApplicationId]			UNIQUEIDENTIFIER NOT NULL,
	[NotificationReceivedDate]			DATETIME   NULL,
	[PaymentRecievedDate]				DATETIME   NULL,
	[CommencementDate]					DATETIME   NULL,
	[CompleteDate]						DATETIME   NULL,
	[TransmittedDate]					DATETIME   NULL,
	[AcknowledgedDate]					DATETIME   NULL,
	[DecisionDate]						DATETIME   NULL,
	[NameOfOfficer]						nvarchar(256) NULL,
    [RowVersion]						ROWVERSION       NOT NULL,
    CONSTRAINT [PK_NotificationAssessment] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Update complete.';


GO
