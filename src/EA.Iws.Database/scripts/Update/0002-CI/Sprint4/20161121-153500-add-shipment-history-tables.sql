CREATE TABLE [ImportNotification].[ShipmentNumberHistory] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [NumberOfShipments] INT NOT NULL,
    [DateChanged] DATE NOT NULL,
    [ImportNotificationId] UNIQUEIDENTIFIER NOT NULL,
    [RowVersion] ROWVERSION NOT NULL, 
	CONSTRAINT [PK_ImportShipmentNumberHistory] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT FK_ImportShipmentNumberHistory_ImportNotification FOREIGN KEY ([ImportNotificationId]) REFERENCES [ImportNotification].[Notification]([Id]),
);

GO

CREATE TABLE [Notification].[ShipmentNumberHistory] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [NumberOfShipments] INT NOT NULL,
    [DateChanged] DATE NOT NULL,
    [NotificationId] UNIQUEIDENTIFIER NOT NULL,
    [RowVersion] ROWVERSION NOT NULL, 
	CONSTRAINT [PK_ExportShipmentNumberHistory] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT FK_ExportShipmentNumberHistory_Notification FOREIGN KEY ([NotificationId]) REFERENCES [Notification].[Notification]([Id]),
);