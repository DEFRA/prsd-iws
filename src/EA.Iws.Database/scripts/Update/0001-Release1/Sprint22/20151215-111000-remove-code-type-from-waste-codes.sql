ALTER TABLE [ImportNotification].[WasteCode]
	DROP CONSTRAINT FK_ImportNotificationWasteCode_LookupCodeType;
GO

ALTER TABLE [ImportNotification].[WasteCode]
	DROP COLUMN [Type];
GO