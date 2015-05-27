
CREATE TABLE [Business].[ShipmentInfo] (
    [Id]                UNIQUEIDENTIFIER NOT NULL,
    [NumberOfShipments] INT              NOT NULL,
    [Quantity]          DECIMAL (18, 4)  NOT NULL,
    [Units]             INT				 NOT NULL,
    [FirstDate]         DATE             NOT NULL,
    [LastDate]          DATE             NOT NULL,
    [NotificationId]    UNIQUEIDENTIFIER NOT NULL,
    [RowVersion] ROWVERSION NOT NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ShipmentInfo_Notification] FOREIGN KEY ([NotificationId]) REFERENCES [Notification].[Notification] ([Id])
);

GO
PRINT N'Add Shipment Info table.';


GO
