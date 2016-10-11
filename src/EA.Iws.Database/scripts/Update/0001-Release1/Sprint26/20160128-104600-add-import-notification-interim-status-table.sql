CREATE TABLE [ImportNotification].[InterimStatus]
(
	[Id] UNIQUEIDENTIFIER CONSTRAINT PK_ImportNotification_InterimStatus PRIMARY KEY NOT NULL,
	[IsInterim] BIT NOT NULL,
	[ImportNotificationId] UNIQUEIDENTIFIER CONSTRAINT FK_InterimStatus_ImportNotification FOREIGN KEY REFERENCES [ImportNotification].[Notification]([Id]) NOT NULL,
	[RowVersion] ROWVERSION NOT NULL
);
GO

INSERT INTO [ImportNotification].[InterimStatus]
(
	[Id],
	[ImportNotificationId],
	[IsInterim]
)
SELECT		(SELECT Cast(Cast(Newid() AS BINARY(10)) + Cast(Getdate() AS BINARY(6)) AS UNIQUEIDENTIFIER)),
			[N].[Id],
			CASE WHEN FC.[NumberOfFacilities] > 1 THEN 1
			ELSE 0
			END

FROM		[ImportNotification].[Notification] AS N

INNER JOIN	
			(
				SELECT	COUNT(*) AS [NumberOfFacilities],
						ImportNotificationId
				
				FROM [ImportNotification].[Facility] AS F

				LEFT JOIN [ImportNotification].[FacilityCollection] AS FC
				ON F.[FacilityCollectionId] = FC.[Id]

				GROUP BY (FC.[ImportNotificationId])
			) AS FC

ON			FC.ImportNotificationId = N.Id

WHERE	[N].[Id] NOT IN (SELECT ImportNotificationId FROM [ImportNotification].[InterimStatus])


INSERT INTO [ImportNotification].[InterimStatus]
(
	[Id],
	[ImportNotificationId],
	[IsInterim]
)
SELECT		(SELECT Cast(Cast(Newid() AS BINARY(10)) + Cast(Getdate() AS BINARY(6)) AS UNIQUEIDENTIFIER)),
			[N].[Id],
			0

FROM		[ImportNotification].[Notification] AS N

WHERE	[N].[Id] NOT IN (SELECT ImportNotificationId FROM [ImportNotification].[InterimStatus])