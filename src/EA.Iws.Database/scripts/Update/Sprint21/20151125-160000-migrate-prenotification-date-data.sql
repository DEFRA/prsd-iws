UPDATE M
SET PreNotificationDate = SC.ChangeDate
FROM [Notification].[Movement] AS M
INNER JOIN [Notification].[MovementStatusChange] AS SC
ON SC.MovementId = M.Id
WHERE SC.Status = (SELECT [Id] FROM [Lookup].[MovementStatus] WHERE Status = 'Submitted')
GO
