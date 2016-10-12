CREATE TABLE [Notification].[NotificationDecision](
    [Id] [uniqueidentifier] NOT NULL CONSTRAINT [PK_NotificationDecision] PRIMARY KEY,
    [NotificationApplicationId] [uniqueidentifier] NOT NULL CONSTRAINT [FK_NotificationDecision_NotificationApplication] FOREIGN KEY REFERENCES [Notification].[Notification] ([Id]),
    [RowVersion] [timestamp] NOT NULL,
    [DecisionMade] [datetime] NULL,
    [ConsentedFrom] [datetime] NULL,
    [ConsentedTo] [datetime] NULL,
    [DecisionType] [int] NULL,
    [ConditionsOfConsent] [varchar](max) NULL
    );
GO

CREATE TABLE [Notification].[NotificationDates] (
    [Id]								UNIQUEIDENTIFIER NOT NULL CONSTRAINT [PK_NotificationDates] PRIMARY KEY,
    [NotificationApplicationId] [uniqueidentifier] NOT NULL CONSTRAINT [FK_NotificationDates_NotificationApplication] FOREIGN KEY REFERENCES [Notification].[Notification] ([Id]),
    [NotificationReceivedDate]			DATETIME   NULL,
    [PaymentReceivedDate]				DATETIME   NULL,
    [CommencementDate]					DATETIME   NULL,
    [CompleteDate]						DATETIME   NULL,
    [TransmittedDate]					DATETIME   NULL,
    [AcknowledgedDate]					DATETIME   NULL,
    [DecisionDate]						DATETIME   NULL,
    [NameOfOfficer]						nvarchar(256) NULL,
    [RowVersion]						ROWVERSION       NOT NULL
);

INSERT INTO [Notification].[NotificationDates]
(
    [Id],
    [NotificationApplicationId] ,
    [NotificationReceivedDate]	,
    [PaymentReceivedDate]		,
    [CommencementDate]			,
    [CompleteDate]				,
    [TransmittedDate]			,
    [AcknowledgedDate]			,
    [DecisionDate]				,
    [NameOfOfficer]	
)
SELECT
    (select CAST(CAST(NEWID() AS BINARY(10)) + CAST(GETDATE() AS BINARY(6)) AS UNIQUEIDENTIFIER)),
    [NotificationApplicationId] ,
    [NotificationReceivedDate]	,
    [PaymentReceivedDate]		,
    [CommencementDate]			,
    [CompleteDate]				,
    [TransmittedDate]			,
    [AcknowledgedDate]			,
    [DecisionDate]				,
    [NameOfOfficer]	
FROM [Notification].[NotificationAssessment]

INSERT INTO [Notification].[NotificationDecision]
(
    [Id] ,
    [NotificationApplicationId] ,
    [DecisionMade] ,
    [ConsentedFrom] ,
    [ConsentedTo] ,
    [DecisionType] , 
    [ConditionsOfConsent]
)
SELECT
    (select CAST(CAST(NEWID() AS BINARY(10)) + CAST(GETDATE() AS BINARY(6)) AS UNIQUEIDENTIFIER)),
    [NotificationApplicationId] ,
    [DecisionMade] ,
    [ConsentedFrom] ,
    [ConsentedTo] ,
    [DecisionType] , 
    [ConditionsOfConsent]
FROM [Notification].[NotificationAssessment]

ALTER TABLE [Notification].[NotificationAssessment]
DROP COLUMN [NotificationReceivedDate]

ALTER TABLE [Notification].[NotificationAssessment]
DROP COLUMN [PaymentReceivedDate]

ALTER TABLE [Notification].[NotificationAssessment]
DROP COLUMN [CommencementDate]

ALTER TABLE [Notification].[NotificationAssessment]
DROP COLUMN [CompleteDate]

ALTER TABLE [Notification].[NotificationAssessment]
DROP COLUMN [TransmittedDate]

ALTER TABLE [Notification].[NotificationAssessment]
DROP COLUMN [AcknowledgedDate]

ALTER TABLE [Notification].[NotificationAssessment]
DROP COLUMN [DecisionDate]

ALTER TABLE [Notification].[NotificationAssessment]
DROP COLUMN [NameOfOfficer]

ALTER TABLE [Notification].[NotificationAssessment]
DROP COLUMN [DecisionMade]

ALTER TABLE [Notification].[NotificationAssessment]
DROP COLUMN [ConsentedFrom]

ALTER TABLE [Notification].[NotificationAssessment]
DROP COLUMN [ConsentedTo]

ALTER TABLE [Notification].[NotificationAssessment]
DROP COLUMN [DecisionType]

ALTER TABLE [Notification].[NotificationAssessment]
DROP COLUMN [ConditionsOfConsent]