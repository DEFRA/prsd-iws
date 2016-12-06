CREATE TABLE [Lookup].[FinancialGuaranteeDecision](
	[Id] [int] NOT NULL,
	[Description] [nvarchar](30) NOT NULL,
 CONSTRAINT [PK_FinancialGuaranteeDecision] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];

GO

INSERT INTO [Lookup].[FinancialGuaranteeDecision] ([Id], [Description])
SELECT 1, 'None'
UNION ALL SELECT 2, 'Refused'
UNION ALL SELECT 3, 'Approved'
UNION ALL SELECT 4, 'Released';

GO

ALTER TABLE [Notification].[FinancialGuarantee]
ADD [Decision] INT NULL CONSTRAINT [FK_FinancialGuarantee_FinancialGuaranteeDecision] FOREIGN KEY([Decision]) REFERENCES [Lookup].[FinancialGuaranteeDecision]([Id]);
GO

UPDATE [Notification].[FinancialGuarantee]
SET [Decision] = 1 -- None
WHERE [Status] IN (1, 2, 3); -- Awaiting application / application received / application complete

UPDATE [Notification].[FinancialGuarantee]
SET [Decision] = 2 -- Refused
WHERE [Status] = 5; -- Refused

UPDATE [Notification].[FinancialGuarantee]
SET [Decision] = 3 -- Approved
WHERE [Status] = 4; -- Approved

UPDATE [Notification].[FinancialGuarantee]
SET [Decision] = 4 -- Released
WHERE [Status] = 6; -- Released

GO

ALTER TABLE [Notification].[FinancialGuarantee]
ALTER COLUMN [Decision] INT NOT NULL;
GO