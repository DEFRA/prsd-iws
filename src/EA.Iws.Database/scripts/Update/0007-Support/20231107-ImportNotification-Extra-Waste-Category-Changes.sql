ALTER TABLE [ImportNotification].[WasteType] ADD WasteCategoryType INT NULL;
GO

ALTER TABLE [ImportNotification].[WasteType]
ADD CONSTRAINT FK_WasteCategoryType
FOREIGN KEY (WasteCategoryType) REFERENCES [Lookup].[WasteCategoryType](Id)
GO

CREATE TABLE [ImportNotification].[WasteComponentInfo](
	[Id] [uniqueidentifier]				NOT NULL,
	[RowVersion] [timestamp]			NOT NULL,
	[WasteComponentType] [int]			NOT NULL,	
	[NotificationId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_WasteComponentInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [ImportNotification].[WasteComponentInfo]  WITH CHECK ADD  CONSTRAINT [FK_NotificationWasteComponentInfo_WasteComponentType] FOREIGN KEY([WasteComponentType])
REFERENCES [Lookup].[WasteComponentType] ([Id])
GO

ALTER TABLE [ImportNotification].[WasteComponentInfo] CHECK CONSTRAINT [FK_NotificationWasteComponentInfo_WasteComponentType]
GO

ALTER TABLE [ImportNotification].[WasteComponentInfo]  WITH CHECK ADD  CONSTRAINT [FK_WasteComponentInfo_Notification] FOREIGN KEY([NotificationId])
REFERENCES [ImportNotification].[Notification] ([Id])
GO

ALTER TABLE [ImportNotification].[WasteComponentInfo] CHECK CONSTRAINT [FK_WasteComponentInfo_Notification]
GO
