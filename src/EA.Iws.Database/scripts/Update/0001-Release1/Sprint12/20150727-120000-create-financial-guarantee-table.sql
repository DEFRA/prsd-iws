PRINT 'Creating financial guarantee table';
GO

CREATE TABLE [Notification].[FinancialGuarantee]
(
	[Id] UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_FinancialGuarantee PRIMARY KEY,
	[Status] INT NOT NULL,
	[ReceivedDate] DATETIME2(0) NULL,
	[CompletedDate] DATETIME2(0) NULL,
	[NotificationAssessmentId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_FinancialGuarantee_NotificationAssessment FOREIGN KEY REFERENCES [Notification].[NotificationAssessment]([Id]),
	[CreatedDate] DATETIME2(0) NULL,
	[RowVersion] ROWVERSION NOT NULL
);
GO

PRINT 'Creating financial guarantee status table';
GO

CREATE TABLE [Notification].[FinancialGuaranteeStatusChange]
(
	[Id] UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_FinancialGuaranteeStatusChange PRIMARY KEY,
	[FinancialGuaranteeId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_FinancialGuaranteeStatusChange_FinancialGuarantee FOREIGN KEY REFERENCES [Notification].[FinancialGuarantee]([Id]),
	[Status] INT NOT NULL,
	[UserId] NVARCHAR(128) NOT NULL CONSTRAINT FK_FinancialGuaranteeStatusChange_User FOREIGN KEY REFERENCES [Identity].[AspNetUsers]([Id]),
	[ChangeDate] DATETIME2(0) NOT NULL 
	CONSTRAINT DF_FinancialGuaranteeStatusChange_ChangeDate DEFAULT GETDATE(),
	[RowVersion] ROWVERSION NOT NULL
);
GO

CREATE NONCLUSTERED INDEX IX_FinancialGuaranteeStatusChange_ChangeDate ON [Notification].[FinancialGuaranteeStatusChange] ([ChangeDate]);
GO

CREATE NONCLUSTERED INDEX IX_FinancialGuaranteeStatusChange_FinancialGuaranteeId ON [Notification].[FinancialGuaranteeStatusChange] ([FinancialGuaranteeId]);
GO