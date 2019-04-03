GO

CREATE TABLE [Notification].[Audit](
	[Id] [uniqueidentifier] NOT NULL,
	[NotificationId] [uniqueidentifier] NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[Screen] [int] NOT NULL,
	[Type] [int] NOT NULL,
	[DateAdded] [datetimeoffset](0) NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
 CONSTRAINT [PK_NotificationAudit] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Notification].[Audit]  WITH CHECK ADD  CONSTRAINT [FK_NotificationAudit_Notification] FOREIGN KEY([NotificationId])
REFERENCES [Notification].[Notification] ([Id])
GO

ALTER TABLE [Notification].[Audit] CHECK CONSTRAINT [FK_NotificationAudit_Notification]
GO

ALTER TABLE [Notification].[Audit]  WITH CHECK ADD  CONSTRAINT [FK_NotificationAudit_User] FOREIGN KEY([UserId])
REFERENCES [Identity].[AspNetUsers] ([Id])
GO

ALTER TABLE [Notification].[Audit] CHECK CONSTRAINT [FK_NotificationAudit_User]
GO

ALTER TABLE [Notification].[Audit]  WITH CHECK ADD  CONSTRAINT [FK_NotificationAudit_Screen] FOREIGN KEY([Screen])
REFERENCES [Lookup].[Screen] ([Id])
GO

ALTER TABLE [Notification].[Audit] CHECK CONSTRAINT [FK_NotificationAudit_Screen]
GO

ALTER TABLE [Notification].[Audit]  WITH CHECK ADD  CONSTRAINT [FK_NotificationAudit_Type] FOREIGN KEY([Type])
REFERENCES [Lookup].[AuditType] ([Id])
GO

ALTER TABLE [Notification].[Audit] CHECK CONSTRAINT [FK_NotificationAudit_Type]
GO


