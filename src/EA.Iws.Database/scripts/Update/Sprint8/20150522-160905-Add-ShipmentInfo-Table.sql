
CREATE TABLE [Business].[ShipmentInfo] (
    [Id]                INT              NOT NULL,
    [NumberOfShipments] INT              NOT NULL,
    [Quanity]           DECIMAL (18)     NOT NULL,
    [Units]             NVARCHAR (50)    NOT NULL,
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
