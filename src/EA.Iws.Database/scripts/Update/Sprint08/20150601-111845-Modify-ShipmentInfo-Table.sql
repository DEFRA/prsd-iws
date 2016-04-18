

GO
PRINT N'Altering [Business].[ShipmentInfo]...';


GO
ALTER TABLE [Business].[ShipmentInfo] DROP COLUMN [FirstDate], COLUMN [LastDate], COLUMN [NumberOfShipments], COLUMN [Quantity], COLUMN [Units];


GO
PRINT N'Altering [Business].[ShipmentInfo]...';


GO
ALTER TABLE [Business].[ShipmentInfo] ADD 
    [NumberOfShipments] INT              NULL,
    [Quantity]          DECIMAL (18, 4)  NULL,
    [Units]             INT              NULL,
    [FirstDate]         DATE             NULL,
    [LastDate]          DATE             NULL



GO
PRINT N'Update complete.';


GO
