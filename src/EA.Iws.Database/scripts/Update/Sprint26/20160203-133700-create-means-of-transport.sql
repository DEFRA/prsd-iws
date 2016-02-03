CREATE TABLE [Notification].[MeansOfTransport](
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[NotificationId] UNIQUEIDENTIFIER NOT NULL,
	[MeansOfTransport] NVARCHAR(512) NOT NULL,
	[RowVersion] ROWVERSION NOT NULL,
 CONSTRAINT [PK_NotificationMeansOfTransport] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];

GO

ALTER TABLE [Notification].[MeansOfTransport]  WITH CHECK ADD  CONSTRAINT [FK_NotificationMeansOfTransport_Notification] FOREIGN KEY([NotificationId])
REFERENCES [Notification].[Notification] ([Id]);
GO

ALTER TABLE [Notification].[MeansOfTransport] CHECK CONSTRAINT [FK_NotificationMeansOfTransport_Notification];
GO

INSERT INTO [Notification].[MeansOfTransport]
(
	[Id],
	[NotificationId],
	[MeansOfTransport]
)
SELECT
	(SELECT Cast(Cast(Newid() AS BINARY(10)) + Cast(Getdate() AS BINARY(6)) AS UNIQUEIDENTIFIER)) AS [Id],
	[Id] AS [NotificationId],
	[MeansOfTransport]
FROM
	[Notification].[Notification]
WHERE
	[MeansOfTransport] IS NOT NULL;

GO

ALTER TABLE [Notification].[Notification]
DROP COLUMN [MeansOfTransport];
GO