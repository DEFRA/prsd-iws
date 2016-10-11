CREATE TABLE [Notification].[MovementStatusChange]
(
	[Id] [uniqueidentifier] NOT NULL,
	[MovementId] [uniqueidentifier] NOT NULL,
	[Status] [int] NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[ChangeDate] [datetime2](0) NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
 CONSTRAINT [PK_MovementStatusChange] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Notification].[MovementStatusChange]  WITH CHECK ADD  CONSTRAINT [FK_MovementStatusChange_Movement] FOREIGN KEY([MovementId])
REFERENCES [Notification].[Movement] ([Id])
GO

ALTER TABLE [Notification].[MovementStatusChange] CHECK CONSTRAINT [FK_MovementStatusChange_Movement]
GO

ALTER TABLE [Notification].[MovementStatusChange]  WITH CHECK ADD  CONSTRAINT [FK_MovementStatusChange_Status] FOREIGN KEY([Status])
REFERENCES [Lookup].[MovementStatus] ([Id])
GO

ALTER TABLE [Notification].[MovementStatusChange] CHECK CONSTRAINT [FK_MovementStatusChange_Status]
GO

ALTER TABLE [Notification].[MovementStatusChange]  WITH CHECK ADD  CONSTRAINT [FK_MovementStatusChange_User] FOREIGN KEY([UserId])
REFERENCES [Identity].[AspNetUsers] ([Id])
GO

ALTER TABLE [Notification].[MovementStatusChange] CHECK CONSTRAINT [FK_MovementStatusChange_User]
GO