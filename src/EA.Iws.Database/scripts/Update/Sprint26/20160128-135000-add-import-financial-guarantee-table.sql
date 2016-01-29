CREATE TABLE [ImportNotification].[FinancialGuarantee]
(
	[Id] UNIQUEIDENTIFIER CONSTRAINT PK_ImportNotification_FinancialGuarantee PRIMARY KEY NOT NULL,
	[Status] INT CONSTRAINT FK_ImportNotification_FinancialGuarantee_FinancialGuaranteeStatus FOREIGN KEY 
	REFERENCES [Lookup].[FinancialGuaranteeStatus]([Id]),
	[ReceivedDate] DATE NOT NULL,
	[CompletedDate] DATE NULL,
	[ImportNotificationId] UNIQUEIDENTIFIER CONSTRAINT FK_ImportNotification_FinancialGuarantee_ImportNotification FOREIGN KEY
	REFERENCES [ImportNotification].[Notification]([Id]),
	[CreatedDate] DATETIMEOFFSET(0) NULL,
	[RowVersion] ROWVERSION NOT NULL
);
GO
