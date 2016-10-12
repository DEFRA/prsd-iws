INSERT INTO [Notification].[NotificationAssessment]
(
    [Id],
    [NotificationApplicationId],
    [Status]
)
SELECT
    (select CAST(CAST(NEWID() AS BINARY(10)) + CAST(GETDATE() AS BINARY(6)) AS UNIQUEIDENTIFIER)) as [Id],
    [Id] as [NotificationApplicationId],
    1 as [Status]
FROM
    [Notification].[Notification]
WHERE
    Id NOT IN (SELECT NotificationApplicationId FROM [Notification].[NotificationAssessment])