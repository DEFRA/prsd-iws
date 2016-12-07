ALTER TABLE [ImportNotification].[WasteType]
ADD [ChemicalCompositionType] INT NULL CONSTRAINT FK_ImportNotificationWasteType_ChemicalCompositionType
FOREIGN KEY REFERENCES [Lookup].[ChemicalCompositionType]([Id]);
GO

UPDATE [ImportNotification].[WasteType]
SET [ChemicalCompositionType] = 4; -- Set all to 'Other'
GO

ALTER TABLE [ImportNotification].[WasteType]
ALTER COLUMN [ChemicalCompositionType] INT NOT NULL;
GO

-- Set notifications to 'Wood' type which have waste code AC170 set
UPDATE [ImportNotification].[WasteType]
SET [ChemicalCompositionType] = 3
WHERE [ImportNotificationId] IN (
	SELECT N.[Id]
	FROM [ImportNotification].[Notification] N
	INNER JOIN [ImportNotification].[WasteType] WT ON WT.ImportNotificationId = N.Id
	INNER JOIN [ImportNotification].[WasteCode] WC ON WC.WasteTypeId = WT.Id
	INNER JOIN [Lookup].[WasteCode] LWC ON LWC.Id = WC.WasteCodeId
	WHERE LWC.Code = 'AC170'
);
GO