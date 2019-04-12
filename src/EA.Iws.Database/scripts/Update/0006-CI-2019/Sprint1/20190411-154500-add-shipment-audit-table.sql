CREATE TABLE [Notification].[MovementAudit](
	[Id] [uniqueidentifier] NOT NULL,
	[NotificationId] [uniqueidentifier] NOT NULL,
	[ShipmentNumber] [int] NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[Type] [int] NOT NULL,
	[DateAdded] [datetimeoffset](0) NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
 CONSTRAINT [PK_MovementAudit] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Notification].[MovementAudit]  WITH CHECK ADD  CONSTRAINT [FK_MovementAudit_Notification] FOREIGN KEY([NotificationId])
REFERENCES [Notification].[Notification] ([Id])
GO

ALTER TABLE [Notification].[MovementAudit] CHECK CONSTRAINT [FK_MovementAudit_Notification]
GO

ALTER TABLE [Notification].[MovementAudit]  WITH CHECK ADD  CONSTRAINT [FK_MovementAudit_User] FOREIGN KEY([UserId])
REFERENCES [Identity].[AspNetUsers] ([Id])
GO

ALTER TABLE [Notification].[MovementAudit] CHECK CONSTRAINT [FK_MovementAudit_User]
GO

ALTER TABLE [Notification].[MovementAudit]  WITH CHECK ADD  CONSTRAINT [FK_MovementAudit_Type] FOREIGN KEY([Type])
REFERENCES [Lookup].[MovementAuditType] ([Id])
GO

ALTER TABLE [Notification].[MovementAudit] CHECK CONSTRAINT [FK_MovementAudit_Type]
GO

CREATE TABLE [ImportNotification].[MovementAudit](
	[Id] [uniqueidentifier] NOT NULL,
	[NotificationId] [uniqueidentifier] NOT NULL,
	[ShipmentNumber] [int] NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[Type] [int] NOT NULL,
	[DateAdded] [datetimeoffset](0) NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
 CONSTRAINT [PK_MovementAudit] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [ImportNotification].[MovementAudit]  WITH CHECK ADD  CONSTRAINT [FK_ImportMovementAudit_Notification] FOREIGN KEY([NotificationId])
REFERENCES [ImportNotification].[Notification] ([Id])
GO

ALTER TABLE [ImportNotification].[MovementAudit] CHECK CONSTRAINT [FK_ImportMovementAudit_Notification]
GO

ALTER TABLE [ImportNotification].[MovementAudit]  WITH CHECK ADD  CONSTRAINT [FK_ImportMovementAudit_User] FOREIGN KEY([UserId])
REFERENCES [Identity].[AspNetUsers] ([Id])
GO

ALTER TABLE [ImportNotification].[MovementAudit] CHECK CONSTRAINT [FK_ImportMovementAudit_User]
GO

ALTER TABLE [ImportNotification].[MovementAudit]  WITH CHECK ADD  CONSTRAINT [FK_ImportMovementAudit_Type] FOREIGN KEY([Type])
REFERENCES [Lookup].[MovementAuditType] ([Id])
GO

ALTER TABLE [ImportNotification].[MovementAudit] CHECK CONSTRAINT [FK_ImportMovementAudit_Type]
GO