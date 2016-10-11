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
ALTER TABLE [Notification].[NotificationAssessment] WITH NOCHECK
    ADD CONSTRAINT [FK_NotificationAssessment_NotificationApplication] FOREIGN KEY ([NotificationApplicationId]) REFERENCES [Notification].[Notification] ([Id]);

GO
PRINT N'Update complete.';


GO
