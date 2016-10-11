CREATE TABLE [Lookup].[ImportFinancialGuaranteeStatus]
(
	[Id] INT CONSTRAINT PK_Lookup_ImportFinancialGuaranteeStatus PRIMARY KEY NOT NULL,
	[Description] NVARCHAR(30) NOT NULL
);
GO

INSERT INTO [Lookup].[ImportFinancialGuaranteeStatus] ([Id], [Description])
VALUES
(1, 'Awaiting application'),
(2, 'Application received'),
(3, 'Application complete'),
(4, 'Approved'),
(5, 'Refused'),
(6, 'Released');
GO

ALTER TABLE [ImportNotification].[FinancialGuarantee]
DROP CONSTRAINT [FK_ImportNotification_FinancialGuarantee_FinancialGuaranteeStatus];

ALTER TABLE [ImportNotification].[FinancialGuarantee]
ADD CONSTRAINT [FK_ImportNotification_FinancialGuarantee_FinancialGuaranteeStatus] FOREIGN KEY 
([Status]) REFERENCES [Lookup].[ImportFinancialGuaranteeStatus]([Id]);

ALTER TABLE [ImportNotification].[FinancialGuaranteeStatusChange]
DROP CONSTRAINT [FK_ImportNotification_FinancialGuaranteeStatusChange_Source];

ALTER TABLE [ImportNotification].[FinancialGuaranteeStatusChange]
ADD CONSTRAINT FK_ImportNotification_FinancialGuaranteeStatusChange_Source FOREIGN KEY 
([Source]) REFERENCES [Lookup].[FinancialGuaranteeStatus]([Id]);

ALTER TABLE [ImportNotification].[FinancialGuaranteeStatusChange]
DROP CONSTRAINT [FK_ImportNotification_FinancialGuaranteeStatusChange_Destination];

ALTER TABLE [ImportNotification].[FinancialGuaranteeStatusChange]
ADD CONSTRAINT FK_ImportNotification_FinancialGuaranteeStatusChange_Destination FOREIGN KEY 
([Destination]) REFERENCES [Lookup].[FinancialGuaranteeStatus]([Id]);