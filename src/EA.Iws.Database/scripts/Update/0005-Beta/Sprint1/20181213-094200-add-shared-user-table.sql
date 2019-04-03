GO
CREATE TABLE [Notification].[SharedUser](
	[Id] [uniqueidentifier] NOT NULL,
	[NotificationId] [uniqueidentifier] NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[DateAdded] [datetimeoffset](0) NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
 CONSTRAINT [PK_SharedUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Notification].[SharedUser]  WITH CHECK ADD  CONSTRAINT [FK_SharedUser_Notification] FOREIGN KEY([NotificationId])
REFERENCES [Notification].[Notification] ([Id])
GO

ALTER TABLE [Notification].[SharedUser] CHECK CONSTRAINT [FK_SharedUser_Notification]
GO

ALTER TABLE [Notification].[SharedUser]  WITH CHECK ADD  CONSTRAINT [FK_SharedUser_User] FOREIGN KEY([UserId])
REFERENCES [Identity].[AspNetUsers] ([Id])
GO

ALTER TABLE [Notification].[SharedUser] CHECK CONSTRAINT [FK_SharedUser_User]
GO

