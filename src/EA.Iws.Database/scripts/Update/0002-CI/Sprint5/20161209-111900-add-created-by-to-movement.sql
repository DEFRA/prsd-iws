ALTER TABLE [Notification].[Movement]
ADD [CreatedBy] NVARCHAR(128) NULL 
	CONSTRAINT FK_NotificationMovement_User FOREIGN KEY 
	REFERENCES [Identity].[AspNetUsers] ([Id]);

GO

UPDATE [Notification].[Movement]
SET [CreatedBy] = A.[UserId]
FROM [Notification].[Movement] M
INNER JOIN [Auditing].[AuditLog] A ON M.Id = A.RecordId
WHERE A.EventType = 0; -- Added

GO

ALTER TABLE [Notification].[Movement]
ALTER COLUMN [CreatedBy] NVARCHAR(128) NOT NULL;

GO

ALTER TABLE [Notification].[MovementReceipt]
ADD [CreatedBy] NVARCHAR(128) NULL 
	CONSTRAINT FK_NotificationMovementReceipt_User FOREIGN KEY 
	REFERENCES [Identity].[AspNetUsers] ([Id]);

GO

UPDATE [Notification].[MovementReceipt]
SET [CreatedBy] = A.[UserId]
FROM [Notification].[MovementReceipt] M
INNER JOIN [Auditing].[AuditLog] A ON M.Id = A.RecordId
WHERE A.EventType = 0; -- Added

GO

ALTER TABLE [Notification].[MovementReceipt]
ALTER COLUMN [CreatedBy] NVARCHAR(128) NOT NULL;

GO

ALTER TABLE [Notification].[MovementOperationReceipt]
ADD [CreatedBy] NVARCHAR(128) NULL 
	CONSTRAINT FK_NotificationMovementOperationReceipt_User FOREIGN KEY 
	REFERENCES [Identity].[AspNetUsers] ([Id]);

GO

UPDATE [Notification].[MovementOperationReceipt]
SET [CreatedBy] = A.[UserId]
FROM [Notification].[MovementOperationReceipt] M
INNER JOIN [Auditing].[AuditLog] A ON M.Id = A.RecordId
WHERE A.EventType = 0; -- Added

GO

ALTER TABLE [Notification].[MovementOperationReceipt]
ALTER COLUMN [CreatedBy] NVARCHAR(128) NOT NULL;

GO