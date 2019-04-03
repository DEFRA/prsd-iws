-- Movement
ALTER TABLE [Notification].[Movement]
ADD [CreatedOnDate] datetime2 NULL;
GO

UPDATE [Notification].[Movement]
SET [CreatedOnDate] = A.[EventDate]
FROM [Notification].[Movement] M
INNER JOIN [Auditing].[AuditLog] A ON M.Id = A.RecordId
WHERE A.EventType = 0;

-- If there was no AuditLog record then set CreatedOnDate to Movement Date
UPDATE [Notification].[Movement]
SET [CreatedOnDate] = [Date]
WHERE [CreatedOnDate] IS NULL;
GO

ALTER TABLE [Notification].[Movement]
ALTER COLUMN [CreatedOnDate] datetime2 NOT NULL;
GO

-- Movement Receipt
ALTER TABLE [Notification].[MovementReceipt]
ADD [CreatedOnDate] datetime2 NULL;
GO

UPDATE [Notification].[MovementReceipt]
SET [CreatedOnDate] = A.[EventDate]
FROM [Notification].[MovementReceipt] MR
INNER JOIN [Auditing].[AuditLog] A ON MR.Id = A.RecordId
WHERE A.EventType = 0;

-- If there was no AuditLog record then set CreatedOnDate to Movement Receipt Date
UPDATE [Notification].[MovementReceipt]
SET [CreatedOnDate] = [Date]
WHERE [CreatedOnDate] IS NULL;
GO

ALTER TABLE [Notification].[MovementReceipt]
ALTER COLUMN [CreatedOnDate] datetime2 NOT NULL;
GO

-- Movement Operation Receipt
ALTER TABLE [Notification].[MovementOperationReceipt]
ADD [CreatedOnDate] datetime2 NULL;
GO

UPDATE [Notification].[MovementOperationReceipt]
SET [CreatedOnDate] = A.[EventDate]
FROM [Notification].[MovementOperationReceipt] MOR
INNER JOIN [Auditing].[AuditLog] A ON MOR.Id = A.RecordId
WHERE A.EventType = 0;

-- If there was no AuditLog record then set CreatedOnDate to Movement Operation Receipt Date
UPDATE [Notification].[MovementOperationReceipt]
SET [CreatedOnDate] = [Date]
WHERE [CreatedOnDate] IS NULL;
GO

ALTER TABLE [Notification].[MovementOperationReceipt]
ALTER COLUMN [CreatedOnDate] datetime2 NOT NULL;
GO

