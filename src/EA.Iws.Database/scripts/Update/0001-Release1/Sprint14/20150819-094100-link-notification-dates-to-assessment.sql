ALTER TABLE [Notification].[NotificationDates]
ADD [NotificationAssessmentId] UNIQUEIDENTIFIER NULL 
    CONSTRAINT FK_NotificationDates_NotificationAssessment 
    FOREIGN KEY REFERENCES [Notification].[NotificationAssessment]([Id])

GO

UPDATE ND
SET
    ND.[NotificationAssessmentId] = NA.[Id]
FROM
    [Notification].[NotificationAssessment] NA
    INNER JOIN [Notification].[NotificationDates] ND on NA.[NotificationApplicationId] = ND.[NotificationApplicationId]

GO

ALTER TABLE [Notification].[NotificationDates]
DROP CONSTRAINT FK_NotificationDates_NotificationApplication

ALTER TABLE [Notification].[NotificationDates]
DROP COLUMN [NotificationApplicationId]

GO

INSERT INTO [Notification].[NotificationDates]
(
    [Id],
    [NotificationAssessmentId]
)
SELECT
    (select CAST(CAST(NEWID() AS BINARY(10)) + CAST(GETDATE() AS BINARY(6)) AS UNIQUEIDENTIFIER)) as [Id],
    [Id] as [NotificationAssessmentId]
FROM
    [Notification].[NotificationAssessment]
WHERE
    Id NOT IN (SELECT [NotificationAssessmentId] FROM [Notification].[NotificationDates])

GO

ALTER TABLE [Notification].[NotificationDates]
ALTER COLUMN [NotificationAssessmentId] UNIQUEIDENTIFIER NOT NULL

GO