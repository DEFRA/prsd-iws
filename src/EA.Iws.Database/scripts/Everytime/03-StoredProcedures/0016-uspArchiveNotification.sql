﻿IF OBJECT_ID('[Notification].[uspArchiveNotification]') IS NULL
    EXEC('CREATE PROCEDURE [Notification].[uspArchiveNotification] AS SET NOCOUNT ON;')
GO

ALTER PROCEDURE [Notification].[uspArchiveNotification]
  @NotificationId UNIQUEIDENTIFIER,
  @CurrentUserId UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;
    
	IF NOT EXISTS (SELECT * FROM [ImportNotification].[Notification] WHERE Id = @NotificationId)
		AND NOT EXISTS (SELECT * FROM [Notification].[Notification] WHERE Id = @NotificationId)
	BEGIN
		SELECT 
			'false' AS IsArchived
		RETURN 
	END

	IF EXISTS (SELECT * FROM [ImportNotification].[Notification] WHERE Id = @NotificationId)
	--ImportNotification
	BEGIN
		IF ((SELECT [IsArchived] FROM [ImportNotification].[Notification] WHERE Id = @NotificationId) = 1)
		BEGIN
			SELECT 
				'false' AS IsArchived
			RETURN 
		END 
		--Notifier
		UPDATE [ImportNotification].[Exporter] 
		SET ContactName = 'Archived', Telephone = '000000', Email = 'archived@archive.com'
		WHERE ImportNotificationId = @NotificationId

		UPDATE [ImportNotification].[Exporter] 
		SET [Name] = 'Archived', Address1 = 'Archived', Address2 = 'Archived', TownOrCity = 'Archived', PostalCode = 'Archived'
		WHERE ImportNotificationId = @NotificationId AND [Type] IN (2,3,4)
		--How do we get the external user?

		UPDATE [ImportNotification].[Producer] 
		SET ContactName = 'Archived', Telephone = '000000', Email = 'archived@archive.com'
		WHERE ImportNotificationId = @NotificationId

		UPDATE [ImportNotification].[Producer] 
		SET [Name] = 'Archived', Address1 = 'Archived', Address2 = 'Archived', TownOrCity = 'Archived', PostalCode = 'Archived'
		WHERE ImportNotificationId = @NotificationId AND [Type] IN (2,3,4)

		--Consignee
		UPDATE [ImportNotification].[Importer] 
		SET ContactName = 'Archived', Telephone = '000000', Email = 'archived@archive.com'
		WHERE ImportNotificationId = @NotificationId
		
		UPDATE [ImportNotification].[Importer] 
		SET [Name] = 'Archived', Address1 = 'Archived', Address2 = 'Archived', TownOrCity = 'Archived', PostalCode = 'Archived'
		WHERE ImportNotificationId = @NotificationId AND [Type] IN (2,3,4)
		
		UPDATE [ImportNotification].[Facility]
		SET ContactName = 'Archived', Telephone = '000000', Email = 'archived@archive.com'
		WHERE FacilityCollectionId IN (SELECT Id FROM [ImportNotification].[FacilityCollection] WHERE ImportNotificationId = @NotificationId)
		
		UPDATE [ImportNotification].[Facility] 
		SET [Name] = 'Archived', Address1 = 'Archived', Address2 = 'Archived', TownOrCity = 'Archived', PostalCode = 'Archived'
		WHERE FacilityCollectionId IN (SELECT Id FROM [ImportNotification].[FacilityCollection] WHERE ImportNotificationId = @NotificationId) AND [Type] IN (2,3,4)
		
		--Is there no Carrier for Imports?
		
		--What recovery info needs archived? No obviously personal details present
		
		DELETE FROM [FileStore].[File] WHERE Id IN (
			SELECT FileId FROM [ImportNotification].[MovementPartialRejection] 
				WHERE MovementId IN (SELECT Id FROM [Notification].[Movement] WHERE NotificationId = @NotificationId))

		UPDATE [ImportNotification].[MovementPartialRejection] SET FileId = null
			WHERE MovementId IN (SELECT Id FROM [Notification].[Movement] WHERE NotificationId = @NotificationId)

		UPDATE [ImportNotification].[Notification] SET IsArchived = 1, ArchivedByUserId = @CurrentUserId, ArchivedDate = SYSDATETIMEOFFSET() WHERE Id = @NotificationId
		
		SELECT 'true' AS IsArchived
	END 
	ELSE 
	BEGIN
	--ExportNotification
		IF ((SELECT [IsArchived] FROM [Notification].[Notification] WHERE Id = @NotificationId) = 1)
		BEGIN
			SELECT 'false' AS IsArchived
			RETURN
		END

		--Notifier
		UPDATE [Notification].[Exporter]
		SET FullName = 'Archived', Telephone = '000000', Fax = '000000', Email = 'archived@archive.com'
		WHERE NotificationId = @NotificationId

		UPDATE [Notification].[Exporter] 
		SET [Name] = 'Archived', Address1 = 'Archived', Address2 = 'Archived', TownOrCity = 'Archived', PostalCode = 'Archived', Region = 'Archived'
		WHERE NotificationId = @NotificationId AND [Type] IN (2,3,4)

		UPDATE [Notification].[Producer]
		SET FullName = 'Archived', Telephone = '000000', Fax = '000000', Email = 'archived@archive.com'
		WHERE ProducerCollectionId IN (SELECT Id FROM [Notification].[ProducerCollection] WHERE NotificationId = @NotificationId)

		UPDATE [Notification].[Producer]
		SET [Name] = 'Archived', Address1 = 'Archived', Address2 = 'Archived', TownOrCity = 'Archived', PostalCode = 'Archived'
		WHERE ProducerCollectionId IN (SELECT Id FROM [Notification].[ProducerCollection] WHERE NotificationId = @NotificationId)
		AND [Type] IN (2,3,4)
		
		--Consignee
		UPDATE [Notification].[Importer]
		SET FullName = 'Archived', Telephone = '000000', Fax = '000000', Email = 'archived@archive.com'
		WHERE NotificationId = @NotificationId

		UPDATE [Notification].[Importer]
		SET [Name] = 'Archived', Address1 = 'Archived', Address2 = 'Archived', TownOrCity = 'Archived', PostalCode = 'Archived'
		WHERE NotificationId = @NotificationId AND [Type] IN (2,3,4)
				
		UPDATE [Notification].[Facility]
		SET FullName = 'Archived', Telephone = '000000', Fax = '000000', Email = 'archived@archive.com'
		WHERE FacilityCollectionId IN (SELECT Id FROM [Notification].[FacilityCollection] WHERE NotificationId = @NotificationId)

		UPDATE [Notification].[Facility]
		SET [Name] = 'Archived', Address1 = 'Archived', Address2 = 'Archived', TownOrCity = 'Archived', PostalCode = 'Archived'
		WHERE FacilityCollectionId IN (SELECT Id FROM [Notification].[FacilityCollection] WHERE NotificationId = @NotificationId)
		AND [Type] IN (2,3,4)

		UPDATE [Notification].[Carrier]
		SET FullName = 'Archived', Telephone = '000000', Fax = '000000', Email = 'archived@archive.com'
		WHERE CarrierCollectionId IN (SELECT Id FROM [Notification].[CarrierCollection] WHERE NotificationId = @NotificationId)

		UPDATE [Notification].[Carrier]
		SET [Name] = 'Archived', Address1 = 'Archived', Address2 = 'Archived', TownOrCity = 'Archived', PostalCode = 'Archived'
		WHERE CarrierCollectionId IN (SELECT Id FROM [Notification].[CarrierCollection] WHERE NotificationId = @NotificationId)
		AND [Type] IN (2,3,4)

		DECLARE @tmpAnnexCollectionFiles TABLE (FileId UNIQUEIDENTIFIER);

		INSERT INTO @tmpAnnexCollectionFiles (FileId) (
			SELECT ProcessOfGenerationId FROM [Notification].[AnnexCollection] WHERE NotificationId = @NotificationId AND ProcessOfGenerationId IS NOT NULL
			UNION
			SELECT WasteCompositionId FROM [Notification].[AnnexCollection] WHERE NotificationId = @NotificationId AND WasteCompositionId IS NOT NULL
			UNION
			SELECT TechnologyEmployedId FROM [Notification].[AnnexCollection] WHERE NotificationId = @NotificationId AND TechnologyEmployedId IS NOT NULL
		)
		UPDATE [Notification].[AnnexCollection] SET ProcessOfGenerationId = null, WasteCompositionId = null, TechnologyEmployedId = null WHERE NotificationId = @NotificationId

		DELETE FROM [FileStore].[File] WHERE Id IN (SELECT FileId FROM @tmpAnnexCollectionFiles)

		DELETE FROM [FileStore].[File] where Id IN (SELECT FileId FROM [Notification].[Movement] WHERE NotificationId = @NotificationId)

		DELETE FROM [FileStore].[File] where Id IN (SELECT FileId FROM [Notification].[MovementOperationReceipt] 
			WHERE MovementId IN (select Id FROM [Notification].[Movement] WHERE NotificationId = @NotificationId))

		DELETE FROM [FileStore].[File] WHERE Id IN (SELECT FileId FROM [Notification].[MovementPartialRejection]
			WHERE MovementId IN (SELECT Id FROM [Notification].[Movement] WHERE NotificationId = @NotificationId))

		DELETE FROM [FileStore].[File] WHERE Id IN (SELECT FileId FROM [Notification].[MovementReceipt] 
			WHERE MovementId IN (SELECT Id FROM [Notification].[Movement] WHERE NotificationId = @NotificationId))

		DELETE FROM [FileStore].[File] WHERE Id IN (SELECT FileId FROM [Notification].[MovementRejection]
			WHERE MovementId IN (SELECT Id FROM [Notification].[Movement] WHERE NotificationId = @NotificationId))

		UPDATE [Notification].[Movement] SET FileId = null WHERE NotificationId = @NotificationId

		UPDATE [Notification].[MovementOperationReceipt] SET FileId = null 
			WHERE MovementId IN (select Id FROM [Notification].[Movement] WHERE NotificationId = @NotificationId)

		UPDATE [Notification].[MovementPartialRejection]  SET FileId = null 
			WHERE MovementId IN (SELECT Id FROM [Notification].[Movement] WHERE NotificationId = @NotificationId)

		UPDATE [Notification].[MovementReceipt]  SET FileId = null 
			WHERE MovementId IN (SELECT Id FROM [Notification].[Movement] WHERE NotificationId = @NotificationId)

		UPDATE [Notification].[MovementRejection] SET FileId = null 
			WHERE MovementId IN (SELECT Id FROM [Notification].[Movement] WHERE NotificationId = @NotificationId)

		UPDATE [Notification].[Notification] SET IsArchived = 1, ArchivedByUserId = @CurrentUserId, ArchivedDate = SYSDATETIMEOFFSET() WHERE Id = @NotificationId

		SELECT 'true' as IsArchived
	END
END
GO