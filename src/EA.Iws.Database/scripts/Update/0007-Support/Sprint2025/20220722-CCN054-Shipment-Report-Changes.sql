ALTER TABLE [Reports].[ShipmentsCache] ADD [RejectedQuantity] decimal(18, 4) NULL;
ALTER TABLE [Reports].[ShipmentsCache] ADD [ShipmentRejectedDate] date NULL;
ALTER TABLE [Reports].[ShipmentsCache] ADD [RejectedReason] NVARCHAR(MAX) NULL;
ALTER TABLE [Reports].[ShipmentsCache] ADD [ActionedByExternalUser] NVARCHAR(5) NULL;
