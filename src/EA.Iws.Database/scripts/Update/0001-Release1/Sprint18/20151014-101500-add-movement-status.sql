CREATE TABLE [Lookup].[MovementStatus]
(
	[Id] INT CONSTRAINT PK_MovementStatus PRIMARY KEY NOT NULL,
	[Status] NVARCHAR(64) NOT NULL
);
GO

INSERT INTO [Lookup].[MovementStatus] ([Id], [Status])
VALUES (1, 'New'),
	   (2, 'Submitted'),
	   (3, 'Received'),
	   (4, 'Completed'),
	   (5, 'Rejected'),
	   (6, 'Cancelled');
GO

ALTER TABLE [Notification].[Movement]
ADD [Status] INT NULL;
GO

UPDATE [Notification].[Movement]
SET [Status] = 1;
GO

ALTER TABLE [Notification].[Movement]
ALTER COLUMN [Status] INT NOT NULL;
GO

ALTER TABLE [Notification].[Movement]
ADD CONSTRAINT FK_Movement_MovementStatus FOREIGN KEY ([Status]) REFERENCES [Lookup].[MovementStatus]([Id]);
GO