ALTER TABLE [Notification].[MovementOperationReceipt] 
ADD [MovementId] UNIQUEIDENTIFIER NULL;
GO

UPDATE MOR
SET MOR.[MovementId] = MR.[MovementId]
FROM [Notification].[MovementOperationReceipt] MOR
INNER JOIN [Notification].[MovementReceipt] MR ON MOR.[MovementReceiptId] = MR.[Id];
GO

ALTER TABLE [Notification].[MovementOperationReceipt]
ALTER COLUMN [MovementId] UNIQUEIDENTIFIER NOT NULL;
GO

ALTER TABLE [Notification].[MovementOperationReceipt]
ADD CONSTRAINT FK_MovementOperationReceipt_Movement 
	FOREIGN KEY ([MovementId]) REFERENCES [Notification].[Movement]([Id]);
GO

ALTER TABLE [Notification].[MovementOperationReceipt]
DROP CONSTRAINT FK_MovementOperationReceipt_MovementReceipt;
GO

ALTER TABLE [Notification].[MovementOperationReceipt]
DROP COLUMN [MovementReceiptId];
GO