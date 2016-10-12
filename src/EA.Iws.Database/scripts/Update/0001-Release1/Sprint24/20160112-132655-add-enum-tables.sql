GO
CREATE TABLE [Lookup].[FinancialGuaranteeStatus]
(
	[Id] INT CONSTRAINT PK_FinancialGuaranteeStatus PRIMARY KEY NOT NULL,
	[Description] NVARCHAR(30) NOT NULL
);

GO

INSERT INTO [Lookup].[FinancialGuaranteeStatus] ([Id], [Description])
VALUES 
	(1, 'Awaiting application'),
	(2, 'Application received'),
	(3, 'Application complete'),
	(4, 'Approved'),
	(5, 'Refused'),
	(6, 'Released');

GO 

ALTER TABLE [Notification].[FinancialGuarantee]
ADD CONSTRAINT FK_FinancialGuarantee_FinancialGuaranteeStatus FOREIGN KEY ([Status]) REFERENCES [Lookup].[FinancialGuaranteeStatus]([Id]);

GO

GO 

ALTER TABLE [Notification].[FinancialGuaranteeStatusChange]
ADD CONSTRAINT FK_FinancialGuaranteeStatusChange_FinancialGuaranteeStatus FOREIGN KEY ([Status]) REFERENCES [Lookup].[FinancialGuaranteeStatus]([Id]);

GO

CREATE TABLE [Lookup].[InternalUserStatus]
(
	[Id] INT CONSTRAINT PK_InternalUserStatus PRIMARY KEY NOT NULL,
	[Description] NVARCHAR(16) NOT NULL
);

GO

INSERT INTO [Lookup].[InternalUserStatus] ([Id], [Description])
VALUES 
	(0, 'Pending'),
	(1, 'Approved'),
	(2, 'Rejected');

GO

ALTER TABLE [Person].[InternalUser]
ADD CONSTRAINT FK_InternalUser_InternalUserStatus FOREIGN KEY ([Status]) REFERENCES [Lookup].[InternalUserStatus]([Id]);

GO