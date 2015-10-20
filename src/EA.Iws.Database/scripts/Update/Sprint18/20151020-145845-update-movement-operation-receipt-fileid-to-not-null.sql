GO

DELETE FROM [Notification].[MovementOperationReceipt]
      WHERE FileId IS NULL;

GO


GO

ALTER TABLE [Notification].[MovementOperationReceipt]
ALTER COLUMN [FileId] UNIQUEIDENTIFIER NOT NULL;

GO