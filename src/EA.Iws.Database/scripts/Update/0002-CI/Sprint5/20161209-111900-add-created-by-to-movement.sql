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