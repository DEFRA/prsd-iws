CREATE SCHEMA Shipment;

GO

CREATE TABLE [Shipment].[Data]
(
	[Id] UNIQUEIDENTIFIER NOT NULL, 
    [ShipmentNumber] INT NOT NULL,
    [NotificationId] UNIQUEIDENTIFIER NOT NULL, 
    [Date] DATE NULL, 
    [Quantity] DECIMAL(18, 4) NULL, 
    [Units] INT NULL, 
    [NumberOfPackages] INT NULL,
    CONSTRAINT [PK_Data_Shipment] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Data_Notification] FOREIGN KEY ([NotificationId]) REFERENCES [Notification].[Notification] (Id)
)