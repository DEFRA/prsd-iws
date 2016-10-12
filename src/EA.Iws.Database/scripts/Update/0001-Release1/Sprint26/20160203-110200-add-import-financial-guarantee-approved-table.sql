CREATE TABLE [ImportNotification].[FinancialGuaranteeApproval]
(
	[Id] UNIQUEIDENTIFIER CONSTRAINT PK_ImportNotification_FinancialGuaranteeApproval PRIMARY KEY NOT NULL,
	[ImportNotificationId] UNIQUEIDENTIFIER CONSTRAINT FK_ImportNotification_FinancialGuaranteeApproval_ImportNotification FOREIGN KEY
	REFERENCES [ImportNotification].[Notification]([Id]) NOT NULL,
	[DecisionDate] DATE NOT NULL,
	[ValidFrom] DATE NOT NULL,
	[ValidTo] DATE NULL,
	[ActiveLoadsPermitted] INT NOT NULL,
	[ReferenceNumber] NVARCHAR(70) NOT NULL,
	[IsBlanketBond] BIT NOT NULL,
	[RowVersion] ROWVERSION NOT NULL
);
GO

CREATE TABLE [ImportNotification].[FinancialGuaranteeRefusal]
(
	[Id] UNIQUEIDENTIFIER CONSTRAINT PK_ImportNotification_FinancialGuaranteeRefusal PRIMARY KEY NOT NULL,
	[ImportNotificationId] UNIQUEIDENTIFIER CONSTRAINT FK_ImportNotification_FinancialGuaranteeRefusal_ImportNotification FOREIGN KEY
	REFERENCES [ImportNotification].[Notification]([Id]) NOT NULL,
	[DecisionDate] DATE NOT NULL,
	[Reason] NVARCHAR(2048) NOT NULL,
	[RowVersion] ROWVERSION NOT NULL
);
GO

CREATE TABLE [ImportNotification].[FinancialGuaranteeRelease]
(
	[Id] UNIQUEIDENTIFIER CONSTRAINT PK_ImportNotification_FinancialGuaranteeRelease PRIMARY KEY NOT NULL,
	[ImportNotificationId] UNIQUEIDENTIFIER CONSTRAINT FK_ImportNotification_FinancialGuaranteeRelease_ImportNotification FOREIGN KEY
	REFERENCES [ImportNotification].[Notification]([Id]) NOT NULL,
	[DecisionDate] DATE NOT NULL,
	[RowVersion] ROWVERSION NOT NULL
);
GO