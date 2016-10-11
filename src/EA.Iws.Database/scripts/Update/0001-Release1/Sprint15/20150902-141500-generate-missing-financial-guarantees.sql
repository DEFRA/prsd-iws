INSERT INTO [Notification].[FinancialGuarantee]
(
	[Id],
	[Status],
	[NotificationApplicationId],
	[CreatedDate]
)
SELECT	(SELECT Cast(Cast(Newid() AS BINARY(10)) + Cast(Getdate() AS BINARY(6)) AS UNIQUEIDENTIFIER)),
		1,
		[Id],
		GETUTCDATE()
FROM	[Notification].[Notification]
WHERE	[Id] NOT IN (SELECT [NotificationApplicationId] FROM [Notification].[FinancialGuarantee])