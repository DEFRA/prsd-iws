EXEC sp_RENAME '[Reports].[ShipmentsCache].ShipmentRejectedDate' , 'RejectedShipmentDate', 'COLUMN'

UPDATE [Notification].[MovementRejection] SET RejectedQuantity=0, RejectedUnit=1 WHERE RejectedQuantity IS NULL