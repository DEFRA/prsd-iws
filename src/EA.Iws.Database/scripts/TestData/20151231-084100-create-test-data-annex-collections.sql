INSERT INTO [Notification].[AnnexCollection]([Id], [NotificationId])
SELECT 	(Cast(Cast(Newid() AS BINARY(10)) + Cast(Getdate() AS BINARY(6)) AS UNIQUEIDENTIFIER)), 
		[Id]
		FROM [Notification].[Notification] 
		WHERE [Id] NOT IN (SELECT NotificationId FROM [Notification].[AnnexCollection]);
GO