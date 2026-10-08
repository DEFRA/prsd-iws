PRINT N'Altering [Notification].[ShipmentInfo]...';

GO
--Add column for determining if shipment data is being self entered
ALTER TABLE [Notification].[ShipmentInfo] ADD [WillSelfEnterShipmentData] BIT NOT NULL CONSTRAINT [DF_WillSelfEnterShipmentData] DEFAULT '1';

GO
ALTER TABLE [Notification].[ShipmentInfo] DROP CONSTRAINT [DF_WillSelfEnterShipmentData];