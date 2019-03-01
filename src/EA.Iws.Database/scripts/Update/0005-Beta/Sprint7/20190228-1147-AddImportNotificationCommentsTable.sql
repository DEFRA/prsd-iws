CREATE TABLE [ImportNotification].[Comments](
	[Id] [uniqueidentifier] NOT NULL,
	[NotificationId] [uniqueidentifier] NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[ShipmentNumber] [int] NULL,
	[Comment] [nvarchar](500) NOT NULL,
	[DateAdded] [datetimeoffset](0) NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
 CONSTRAINT [PK_ImportNotificationComments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [ImportNotification].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_ImportNotificationComments_Notification] FOREIGN KEY([NotificationId])
REFERENCES [ImportNotification].[Notification] ([Id])
GO

ALTER TABLE [ImportNotification].[Comments] CHECK CONSTRAINT [FK_ImportNotificationComments_Notification]
GO

ALTER TABLE [ImportNotification].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_ImportNotificationComments_User] FOREIGN KEY([UserId])
REFERENCES [Identity].[AspNetUsers] ([Id])
GO

ALTER TABLE [ImportNotification].[Comments] CHECK CONSTRAINT [FK_ImportNotificationComments_User]
GO

