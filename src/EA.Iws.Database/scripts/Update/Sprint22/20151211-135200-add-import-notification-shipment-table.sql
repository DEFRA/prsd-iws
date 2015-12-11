CREATE TABLE [ImportNotification].[Shipment] (
    [Id]						UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_ImportNotificationShipment PRIMARY KEY,
    [NumberOfShipments]			INT              NOT NULL,
    [Quantity]					DECIMAL (18, 4)  NOT NULL,
    [Units]						INT				 NOT NULL
		CONSTRAINT FK_ImportShipmentNotification_Units FOREIGN KEY REFERENCES [Lookup].[ShipmentQuantityUnit]([Id]),
    [FirstDate]					DATE             NOT NULL,
    [LastDate]					DATE             NOT NULL,
    [ImportNotificationId]		UNIQUEIDENTIFIER NOT NULL
		CONSTRAINT FK_ImportNotificationShipment_ImportNotification FOREIGN KEY REFERENCES [ImportNotification].[Notification]([Id]),
    [RowVersion] ROWVERSION NOT NULL, 
);