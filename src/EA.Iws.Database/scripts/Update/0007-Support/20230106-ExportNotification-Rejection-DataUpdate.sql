ALTER TABLE [Reports].[ShipmentsCache] DROP COLUMN ShipmentRejectedDate;

UPDATE [Notification].[MovementRejection] SET RejectedQuantity=0, RejectedUnit=1 WHERE RejectedQuantity IS NULL