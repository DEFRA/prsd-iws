GO
ALTER TABLE [Notification].[NotificationAssessment] ADD [DecisionMade] DATETIME NULL

GO
ALTER TABLE [Notification].[NotificationAssessment] ADD [ConsentedFrom] DATETIME NULL

GO
ALTER TABLE [Notification].[NotificationAssessment] ADD [ConsentedTo] DATETIME NULL

GO
ALTER TABLE [Notification].[NotificationAssessment] ADD [DecisionType] INT NULL

GO
ALTER TABLE [Notification].[NotificationAssessment] ADD [ConditionsOfConsent] VARCHAR(MAX) NULL

PRINT N'Update complete.';
GO
