ALTER TABLE [ImportNotification].[Movement]
ADD CONSTRAINT UX_ImportMovement_Number UNIQUE (NotificationId, Number)
GO

ALTER TABLE [ImportNotification].[MovementReceipt]
ADD CONSTRAINT UX_ImportMovementReceipt_Number UNIQUE (MovementId)
GO

ALTER TABLE [ImportNotification].[MovementRejection]
ADD CONSTRAINT UX_ImportMovementRejection_Number UNIQUE (MovementId)
GO

ALTER TABLE [ImportNotification].[MovementOperationReceipt]
ADD CONSTRAINT UX_ImportMovementOperationReceipt_Number UNIQUE (MovementId)
GO