ALTER TABLE [ImportNotification].[WasteType] ADD WasteCategoryType INT NULL;
GO

ALTER TABLE [ImportNotification].[WasteType]
ADD CONSTRAINT FK_WasteCategoryType
FOREIGN KEY (WasteCategoryType) REFERENCES [Lookup].[WasteCategoryType](Id)
GO

CREATE TABLE [ImportNotification].[WasteComponent](
	[Id] [uniqueidentifier]				NOT NULL,
	[RowVersion] [timestamp]			NOT NULL,
	[WasteComponentType] [int]			NOT NULL,	
	[ImportNotificationId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_WasteComponent] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [ImportNotification].[WasteComponent]  WITH CHECK ADD  CONSTRAINT [FK_NotificationWasteComponent_WasteComponentType] FOREIGN KEY([WasteComponentType])
REFERENCES [Lookup].[WasteComponentType] ([Id])
GO

ALTER TABLE [ImportNotification].[WasteComponent] CHECK CONSTRAINT [FK_NotificationWasteComponent_WasteComponentType]
GO

ALTER TABLE [ImportNotification].[WasteComponent]  WITH CHECK ADD  CONSTRAINT [FK_WasteComponent_Notification] FOREIGN KEY([ImportNotificationId])
REFERENCES [ImportNotification].[Notification] ([Id])
GO

ALTER TABLE [ImportNotification].[WasteComponent] CHECK CONSTRAINT [FK_WasteComponent_Notification]
GO
