-- New ProducerCollection table
CREATE TABLE [Notification].[ProducerCollection](
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[NotificationId] UNIQUEIDENTIFIER NOT NULL,
	[RowVersion] ROWVERSION NOT NULL,
 CONSTRAINT [PK_NotificationProducerCollection] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];

GO

ALTER TABLE [Notification].[ProducerCollection]  WITH CHECK ADD  CONSTRAINT [FK_NotificationProducerCollection_Notification] FOREIGN KEY([NotificationId])
REFERENCES [Notification].[Notification] ([Id]);
GO

ALTER TABLE [Notification].[ProducerCollection] CHECK CONSTRAINT [FK_NotificationProducerCollection_Notification];
GO

-- New foreign key on Producer
ALTER TABLE [Notification].[Producer]
ADD [ProducerCollectionId] UNIQUEIDENTIFIER NULL CONSTRAINT FK_NotificationProducer_ProducerCollection
	FOREIGN KEY REFERENCES [Notification].[ProducerCollection] ([Id]);
GO

-- Migrate data
INSERT INTO [Notification].[ProducerCollection](
	[Id],
	[NotificationId]
)
SELECT
	(SELECT Cast(Cast(Newid() AS BINARY(10)) + Cast(Getdate() AS BINARY(6)) AS UNIQUEIDENTIFIER)) AS [Id],
	[Id] AS [NotificationId]
FROM
	[Notification].[Notification];
GO

UPDATE [Notification].[Producer]
SET [ProducerCollectionId] = PC.[Id]
FROM [Notification].[Producer] P
INNER JOIN [Notification].[ProducerCollection] PC ON P.NotificationId = PC.NotificationId;
GO

-- Update foreign keys on Producer
ALTER TABLE [Notification].[Producer]
ALTER COLUMN [ProducerCollectionId] UNIQUEIDENTIFIER NOT NULL;
GO

ALTER TABLE [Notification].[Producer]
DROP CONSTRAINT FK_Producer_Notification;
GO

DROP INDEX IX_Producer_NotificationId ON [Notification].[Producer];
GO

ALTER TABLE [Notification].[Producer]
DROP COLUMN [NotificationId];
GO