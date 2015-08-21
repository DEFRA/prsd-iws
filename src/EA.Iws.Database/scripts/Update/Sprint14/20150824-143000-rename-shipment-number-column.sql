EXEC sp_RENAME 'Notification.Movement.ShipmentNumber' , 'Number', 'COLUMN';
GO

ALTER TABLE [Notification].[Movement]
ADD RowVersion ROWVERSION NOT NULL;
GO