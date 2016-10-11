CREATE TABLE [ImportNotification].[FinancialGuaranteeStatusChange]
(
	[Id] UNIQUEIDENTIFIER CONSTRAINT PK_ImportNotification_FinancialGuaranteeStatusChange PRIMARY KEY NOT NULL,
	[Source] INT CONSTRAINT FK_ImportNotification_FinancialGuaranteeStatusChange_Source FOREIGN KEY 
	REFERENCES [Lookup].[FinancialGuaranteeStatus]([Id]) NOT NULL,
	[Destination] INT CONSTRAINT FK_ImportNotification_FinancialGuaranteeStatusChange_Destination FOREIGN KEY 
	REFERENCES [Lookup].[FinancialGuaranteeStatus]([Id]) NOT NULL,
	[Date] DATETIMEOFFSET(0) NOT NULL,
	[FinancialGuaranteeId] UNIQUEIDENTIFIER CONSTRAINT FK_ImportNotification_FinancialGuaranteeStatusChange_FinancialGuarantee FOREIGN KEY
	REFERENCES [ImportNotification].[FinancialGuarantee]([Id]) NOT NULL,
	[UserId] UNIQUEIDENTIFIER NOT NULL,
	[RowVersion] ROWVERSION NOT NULL
);
GO
