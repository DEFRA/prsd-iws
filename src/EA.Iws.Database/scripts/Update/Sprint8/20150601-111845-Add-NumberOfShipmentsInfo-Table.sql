

GO
PRINT N'Altering [Business].[ShipmentInfo]...';


GO
ALTER TABLE [Business].[ShipmentInfo] DROP COLUMN [FirstDate], COLUMN [LastDate], COLUMN [NumberOfShipments], COLUMN [Quantity], COLUMN [Units];


GO
PRINT N'Creating [Business].[NumberOfShipmentsInfo]...';


GO
CREATE TABLE [Business].[NumberOfShipmentsInfo] (
    [Id]                UNIQUEIDENTIFIER NOT NULL,
    [NumberOfShipments] INT              NOT NULL,
    [Quantity]          DECIMAL (18, 4)  NOT NULL,
    [Units]             INT              NOT NULL,
    [FirstDate]         DATE             NOT NULL,
    [LastDate]          DATE             NOT NULL,
    [ShipmentInfoId]    UNIQUEIDENTIFIER NOT NULL,
    [RowVersion]        ROWVERSION       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating FK_NumberOfShipmentsInfo_ShipmentInfo...';


GO
ALTER TABLE [Business].[NumberOfShipmentsInfo] WITH NOCHECK
    ADD CONSTRAINT [FK_NumberOfShipmentsInfo_ShipmentInfo] FOREIGN KEY ([ShipmentInfoId]) REFERENCES [Business].[ShipmentInfo] ([Id]);


GO
PRINT N'Checking existing data against newly created constraints';


GO


GO
ALTER TABLE [Business].[NumberOfShipmentsInfo] WITH CHECK CHECK CONSTRAINT [FK_NumberOfShipmentsInfo_ShipmentInfo];


GO
PRINT N'Update complete.';


GO
