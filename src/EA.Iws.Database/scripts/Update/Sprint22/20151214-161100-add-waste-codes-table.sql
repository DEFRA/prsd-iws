CREATE TABLE [ImportNotification].[WasteCode] (
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT PK_ImportNotificationWasteCode PRIMARY KEY,
	[WasteTypeId] [uniqueidentifier] NOT NULL 
		CONSTRAINT FK_ImportNotificationWasteCode_ImportNotificationWasteType FOREIGN KEY REFERENCES [ImportNotification].[WasteType]([Id]),
	[WasteCodeId] [uniqueidentifier] NOT NULL
		CONSTRAINT FK_ImportNotificationWasteCode_LookupWasteCode FOREIGN KEY REFERENCES [Lookup].[WasteCode]([Id]),
	[Type] [int] NOT NULL
			CONSTRAINT FK_ImportNotificationWasteCode_LookupCodeType FOREIGN KEY REFERENCES [Lookup].[CodeType]([Id]),
	[RowVersion] [timestamp] NOT NULL
);