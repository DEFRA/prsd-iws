ALTER TABLE [Notification].[MovementOperationReceipt]
ALTER COLUMN FileId UNIQUEIDENTIFIER NULL;
GO

ALTER TABLE [Notification].[MovementReceipt]
DROP COLUMN [RejectReason];
GO