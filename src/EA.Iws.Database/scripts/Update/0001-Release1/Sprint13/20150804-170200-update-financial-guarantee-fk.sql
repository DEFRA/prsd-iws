ALTER TABLE [Notification].[FinancialGuarantee]
ADD [NotificationApplicationId] uniqueidentifier NULL 
	CONSTRAINT FK_FinancialGuarantee_Notification 
	FOREIGN KEY REFERENCES [Notification].[Notification] ( [Id] )
GO

UPDATE FG 
SET FG.[NotificationApplicationId] = NA.NotificationApplicationId
FROM [Notification].[FinancialGuarantee] FG 
INNER JOIN [Notification].[NotificationAssessment] NA ON FG.NotificationAssessmentId = NA.Id
GO

ALTER TABLE [Notification].[FinancialGuarantee]
ALTER COLUMN [NotificationApplicationId] uniqueidentifier NOT NULL

ALTER TABLE [Notification].[FinancialGuarantee]
DROP CONSTRAINT FK_FinancialGuarantee_NotificationAssessment

ALTER TABLE [Notification].[FinancialGuarantee]
DROP COLUMN [NotificationAssessmentId]