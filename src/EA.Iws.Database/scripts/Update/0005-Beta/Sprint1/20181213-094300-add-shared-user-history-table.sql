GO

CREATE TABLE [Notification].[SharedUserHistory](
	[Id] [uniqueidentifier] NOT NULL,
	[NotificationId] [uniqueidentifier] NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[DateAdded] [datetimeoffset](0) NOT NULL,
	[DateRemoved] [datetimeoffset](0) NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
 CONSTRAINT [PK_SharedUserHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Notification].[SharedUserHistory]  WITH CHECK ADD  CONSTRAINT [FK_SharedUserHistory_Notification] FOREIGN KEY([NotificationId])
REFERENCES [Notification].[Notification] ([Id])
GO

ALTER TABLE [Notification].[SharedUserHistory] CHECK CONSTRAINT [FK_SharedUserHistory_Notification]
GO

ALTER TABLE [Notification].[SharedUserHistory]  WITH CHECK ADD  CONSTRAINT [FK_SharedUserHistory_User] FOREIGN KEY([UserId])
REFERENCES [Identity].[AspNetUsers] ([Id])
GO

ALTER TABLE [Notification].[SharedUserHistory] CHECK CONSTRAINT [FK_SharedUserHistory_User]
GO

