-- New FinancialGuaranteeCollection table
CREATE TABLE [Notification].[FinancialGuaranteeCollection](
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[NotificationId] UNIQUEIDENTIFIER NOT NULL,
	[RowVersion] ROWVERSION NOT NULL,
 CONSTRAINT [PK_FinancialGuaranteeCollection] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];

GO

ALTER TABLE [Notification].[FinancialGuaranteeCollection]  WITH CHECK ADD  CONSTRAINT [FK_FinancialGuaranteeCollection_Notification] FOREIGN KEY([NotificationId])
REFERENCES [Notification].[Notification] ([Id]);
GO

ALTER TABLE [Notification].[FinancialGuaranteeCollection] CHECK CONSTRAINT [FK_FinancialGuaranteeCollection_Notification];
GO

-- New foreign key on FinancialGuarantee
ALTER TABLE [Notification].[FinancialGuarantee]
ADD [FinancialGuaranteeCollectionId] UNIQUEIDENTIFIER NULL CONSTRAINT FK_NotificationFinancialGuarantee_FinancialGuaranteeCollection
	FOREIGN KEY REFERENCES [Notification].[FinancialGuaranteeCollection] ([Id]);
GO

-- Migrate data
INSERT INTO [Notification].[FinancialGuaranteeCollection](
	[Id],
	[NotificationId]
)
SELECT
	(SELECT Cast(Cast(Newid() AS BINARY(10)) + Cast(Getdate() AS BINARY(6)) AS UNIQUEIDENTIFIER)) AS [Id],
	[Id] AS [NotificationId]
FROM
	[Notification].[Notification];
GO

UPDATE [Notification].[FinancialGuarantee]
SET [FinancialGuaranteeCollectionId] = FGC.[Id]
FROM [Notification].[FinancialGuarantee] FG
INNER JOIN [Notification].[FinancialGuaranteeCollection] FGC ON FG.NotificationApplicationId = FGC.NotificationId;
GO

-- Update foreign keys on FinancialGuarantee
ALTER TABLE [Notification].[FinancialGuarantee]
ALTER COLUMN [FinancialGuaranteeCollectionId] UNIQUEIDENTIFIER NOT NULL;
GO

ALTER TABLE [Notification].[FinancialGuarantee]
DROP CONSTRAINT FK_FinancialGuarantee_Notification;
GO

DROP INDEX IX_FinancialGuarantee_NotificationId ON [Notification].[FinancialGuarantee];
GO

ALTER TABLE [Notification].[FinancialGuarantee]
DROP COLUMN [NotificationApplicationId];
GO