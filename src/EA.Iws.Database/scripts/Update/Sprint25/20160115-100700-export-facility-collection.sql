-- New FacilityCollection table
CREATE TABLE [Notification].[FacilityCollection](
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[NotificationId] UNIQUEIDENTIFIER NOT NULL,
	[AllFacilitiesPreconsented] BIT NULL,
	[IsInterim] BIT NULL,
	[RowVersion] ROWVERSION NOT NULL,
 CONSTRAINT [PK_NotificationFacilityCollection] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];

GO

ALTER TABLE [Notification].[FacilityCollection]  WITH CHECK ADD  CONSTRAINT [FK_NotificationFacilityCollection_Notification] FOREIGN KEY([NotificationId])
REFERENCES [Notification].[Notification] ([Id]);
GO

ALTER TABLE [Notification].[FacilityCollection] CHECK CONSTRAINT [FK_NotificationFacilityCollection_Notification];
GO

-- New foreign key on Facility
ALTER TABLE [Notification].[Facility]
ADD [FacilityCollectionId] UNIQUEIDENTIFIER NULL CONSTRAINT FK_NotificationFacility_FacilityCollection
	FOREIGN KEY REFERENCES [Notification].[FacilityCollection] ([Id]);
GO

-- Migrate data
INSERT INTO [Notification].[FacilityCollection](
	[Id],
	[NotificationId]
)
SELECT
	(SELECT Cast(Cast(Newid() AS BINARY(10)) + Cast(Getdate() AS BINARY(6)) AS UNIQUEIDENTIFIER)) AS [Id],
	[Id] AS [NotificationId]
FROM
	[Notification].[Notification];
GO

UPDATE [Notification].[FacilityCollection]
SET [AllFacilitiesPreconsented] = N.[IsPreconsentedRecoveryFacility]
FROM [Notification].[FacilityCollection] FC
INNER JOIN [Notification].[Notification] N ON FC.NotificationId = N.Id;
GO

UPDATE [Notification].[Facility]
SET [FacilityCollectionId] = FC.[Id]
FROM [Notification].[Facility] F
INNER JOIN [Notification].[FacilityCollection] FC ON F.NotificationId = FC.NotificationId;
GO

-- Update foreign keys on Facility
ALTER TABLE [Notification].[Facility]
ALTER COLUMN [FacilityCollectionId] UNIQUEIDENTIFIER NOT NULL;
GO

ALTER TABLE [Notification].[Facility]
DROP CONSTRAINT FK_Facility_Notification;
GO

DROP INDEX IX_Facility_NotificationId ON [Notification].[Facility];
GO

ALTER TABLE [Notification].[Facility]
DROP COLUMN [NotificationId];
GO

-- Remove IsPreconsentedRecoveryFacility
ALTER TABLE [Notification].[Notification]
DROP COLUMN [IsPreconsentedRecoveryFacility];
GO