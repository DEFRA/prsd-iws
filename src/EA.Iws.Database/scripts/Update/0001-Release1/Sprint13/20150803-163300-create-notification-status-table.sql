PRINT 'Creating notification status table';
GO

CREATE TABLE [Notification].[NotificationStatusChange]
(
	[Id] UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_NotificationStatusChange PRIMARY KEY,
	[NotificationAssessmentId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_NotificationStatusChange_NotificationAssessment FOREIGN KEY REFERENCES [Notification].[NotificationAssessment]([Id]),
	[Status] INT NOT NULL,
	[UserId] NVARCHAR(128) NOT NULL CONSTRAINT FK_NotificationStatusChange_User FOREIGN KEY REFERENCES [Identity].[AspNetUsers]([Id]),
	[ChangeDate] DATETIME2(0) NOT NULL 
	CONSTRAINT DF_NotificationStatusChange_ChangeDate DEFAULT GETDATE(),
	[RowVersion] ROWVERSION NOT NULL
);
GO

PRINT 'Add Status column to NotificationAssessment'
GO

ALTER TABLE Notification.NotificationAssessment
ADD [Status] INT NULL
GO

UPDATE Notification.NotificationAssessment
SET [Status] = 1 -- NotSubmitted
GO

ALTER TABLE Notification.NotificationAssessment
ALTER COLUMN [Status] INT NOT NULL
GO