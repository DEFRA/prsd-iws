PRINT N'Altering [Notification].[ShipmentInfo]...';

GO
--Add column for determining if shipment data is being self entered
ALTER TABLE [Notification].[ShipmentInfo] ADD [WillSelfEnterShipmentData] BIT NOT NULL CONSTRAINT [DF_WillSelfEnterShipmentData] DEFAULT '0';

GO
ALTER TABLE [Notification].[ShipmentInfo] DROP CONSTRAINT [DF_WillSelfEnterShipmentData];

PRINT N'Altering [ImportNotification].[Shipment]...';
GO
--Add column for determining if shipment data is being self entered
ALTER TABLE [ImportNotification].[Shipment] ADD [WillSelfEnterShipmentData] BIT NOT NULL CONSTRAINT [DF_WillSelfEnterShipmentData] DEFAULT '0';

GO
ALTER TABLE [ImportNotification].[Shipment] DROP CONSTRAINT [DF_WillSelfEnterShipmentData];