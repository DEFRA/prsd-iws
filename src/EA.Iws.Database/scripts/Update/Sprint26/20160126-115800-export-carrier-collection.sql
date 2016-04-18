-- New FacilityCollection table
CREATE TABLE [Notification].[CarrierCollection](
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[NotificationId] UNIQUEIDENTIFIER NOT NULL,
	[RowVersion] ROWVERSION NOT NULL,
 CONSTRAINT [PK_NotificationCarrierCollection] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];

GO

ALTER TABLE [Notification].[CarrierCollection]  WITH CHECK ADD  CONSTRAINT [FK_NotificationCarrierCollection_Notification] FOREIGN KEY([NotificationId])
REFERENCES [Notification].[Notification] ([Id]);
GO

ALTER TABLE [Notification].[CarrierCollection] CHECK CONSTRAINT [FK_NotificationCarrierCollection_Notification];
GO

-- New foreign key on Facility
ALTER TABLE [Notification].[Carrier]
ADD [CarrierCollectionId] UNIQUEIDENTIFIER NULL CONSTRAINT FK_NotificationCarrier_CarrierCollection
	FOREIGN KEY REFERENCES [Notification].[CarrierCollection] ([Id]);
GO

-- Migrate data
INSERT INTO [Notification].[CarrierCollection](
	[Id],
	[NotificationId]
)
SELECT
	(SELECT Cast(Cast(Newid() AS BINARY(10)) + Cast(Getdate() AS BINARY(6)) AS UNIQUEIDENTIFIER)) AS [Id],
	[Id] AS [NotificationId]
FROM
	[Notification].[Notification];
GO

UPDATE [Notification].[Carrier]
SET [CarrierCollectionId] = CC.[Id]
FROM [Notification].[Carrier] C
INNER JOIN [Notification].[CarrierCollection] CC ON C.NotificationId = CC.NotificationId;
GO

-- Update foreign keys on Facility
ALTER TABLE [Notification].[Carrier]
ALTER COLUMN [CarrierCollectionId] UNIQUEIDENTIFIER NOT NULL;
GO

ALTER TABLE [Notification].[Carrier]
DROP CONSTRAINT FK_Carrier_Notification;
GO

DROP INDEX IX_Carrier_NotificationId ON [Notification].[Carrier];
GO

ALTER TABLE [Notification].[Carrier]
DROP COLUMN [NotificationId];
GO