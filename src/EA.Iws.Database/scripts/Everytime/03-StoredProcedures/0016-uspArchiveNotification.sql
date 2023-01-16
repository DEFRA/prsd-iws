IF OBJECT_ID('[Notification].[uspArchiveNotification]') IS NULL
    EXEC('CREATE PROCEDURE [Notification].[uspArchiveNotification] AS SET NOCOUNT ON;')
GO

ALTER PROCEDURE [Notification].[uspArchiveNotification]
  @NotificationId UNIQUEIDENTIFIER,
  @CurrentUserId UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;
    
    IF EXISTS (select * from [ImportNotification].[Notification] where Id = @NotificationId)
	--ImportNotification
	BEGIN
		IF ((SELECT [IsArchived] from [ImportNotification].[Notification] where Id = @NotificationId) = 1)
		BEGIN
			SELECT 
				INN.Id,
				INN.NotificationNumber,
				LINS.[Description] as [Status],
				INND.NotificationReceivedDate as DateGenerated,
				INE.[Name] as CompanyName,
				'Already archived' as ErrorMessage
			FROM [ImportNotification].[Notification] INN
				INNER JOIN [ImportNotification].[NotificationAssessment] INNA ON INN.Id = INNA .NotificationApplicationId
				INNER JOIN [ImportNotification].[NotificationDates] INND ON INND.NotificationAssessmentId = INNA.Id
				INNER JOIN [Lookup].ImportNotificationStatus LINS ON LINS.Id = INNA.[Status]
				LEFT JOIN [ImportNotification].[Exporter] INE on INN.Id = INE.ImportNotificationId
			WHERE INN.Id = @NotificationId
			RETURN 
		END 
	BEGIN TRAN
		--Notifier
		UPDATE [ImportNotification].[Exporter] 
		SET ContactName = 'Archived', Telephone = '000000', Email = 'archived@archive.com'
		WHERE ImportNotificationId = @NotificationId
		UPDATE [ImportNotification].[Exporter] 
		SET [Name] = 'Archived', Address1 = 'Archived', Address2 = 'Archived', TownOrCity = 'Archived', PostalCode = 'Archived'
		WHERE ImportNotificationId = @NotificationId AND [Type] in (2,3)
		--How do we get the external user?

		UPDATE [ImportNotification].[Producer] 
		SET ContactName = 'Archived', Telephone = '000000', Email = 'archived@archive.com'
		WHERE ImportNotificationId = @NotificationId		
		UPDATE [ImportNotification].[Producer] 
		SET [Name] = 'Archived', Address1 = 'Archived', Address2 = 'Archived', TownOrCity = 'Archived', PostalCode = 'Archived'
		WHERE ImportNotificationId = @NotificationId AND [Type] in (2,3)

		--Consignee
		UPDATE [ImportNotification].[Importer] 
		SET ContactName = 'Archived', Telephone = '000000', Email = 'archived@archive.com'
		WHERE ImportNotificationId = @NotificationId		
		UPDATE [ImportNotification].[Importer] 
		SET [Name] = 'Archived', Address1 = 'Archived', Address2 = 'Archived', TownOrCity = 'Archived', PostalCode = 'Archived'
		WHERE ImportNotificationId = @NotificationId AND [Type] in (2,3)
		
		UPDATE [ImportNotification].[Facility]
		SET ContactName = 'Archived', Telephone = '000000', Email = 'archived@archive.com'
		WHERE FacilityCollectionId IN (Select Id from [ImportNotification].[FacilityCollection] where ImportNotificationId = @NotificationId)		
		UPDATE [ImportNotification].[Facility] 
		SET [Name] = 'Archived', Address1 = 'Archived', Address2 = 'Archived', TownOrCity = 'Archived', PostalCode = 'Archived'
		WHERE FacilityCollectionId IN (Select Id from [ImportNotification].[FacilityCollection] where ImportNotificationId = @NotificationId) AND [Type] in (2,3)
		
		--Is there no Carrier for Imports?
		
		--What recovery info needs archived? No obviously personal details present
		
		DELETE FROM [FileStore].[File] where Id in (select FileId FROM [ImportNotification].[MovementPartialRejection]
			Where MovementId in (select Id FROM [Notification].[Movement] Where NotificationId = @NotificationId))

		update [ImportNotification].[Notification] SET IsArchived = 1, ArchivedByUserId = @CurrentUserId, ArchivedDate = SYSDATETIMEOFFSET() where Id = @NotificationId
		
		SELECT 
			INN.Id,
			INN.NotificationNumber,
			LINS.[Description] as [Status],
			INND.NotificationReceivedDate as DateGenerated,
			INE.[Name] as CompanyName,
			INN.CompetentAuthority
		FROM [ImportNotification].[Notification] INN
			INNER JOIN [ImportNotification].[NotificationAssessment] INNA ON INN.Id = INNA .NotificationApplicationId
			INNER JOIN [ImportNotification].[NotificationDates] INND ON INND.NotificationAssessmentId = INNA.Id
			INNER JOIN [Lookup].ImportNotificationStatus LINS ON LINS.Id = INNA.[Status]
			LEFT JOIN [ImportNotification].[Exporter] INE on INN.Id = INE.ImportNotificationId
		WHERE INN.Id = @NotificationId
	END 
	ELSE 
	BEGIN
	--ExportNotification
		IF ((select COUNT(*) from [Notification].[Notification] where Id = @NotificationId) = 0)
		BEGIN
			SELECT @NotificationId as [Id], 
				null as NotificationNumber, 
				null as [Status],
				null as DateGenerated,
				null as CompanyName,
				'Provided NotificationId does not exist' as ErrorMessage;
			RETURN 
		END 
		IF ((select [IsArchived] from [Notification].[Notification] where Id = @NotificationId) = 1)
		BEGIN
			SELECT
				N.Id,
				N.NotificationNumber,
				LNS.[Description] AS [Status],
				N.CreatedDate as DateGenerated,
				E.[Name] AS CompanyName,
				'Already archived' as ErrorMessage
			FROM
				[Notification].[Notification] N
				INNER JOIN [Notification].[NotificationAssessment] NA ON N.Id = NA.NotificationApplicationId
				INNER JOIN [Lookup].NotificationStatus LNS on LNS.Id = NA.[Status]
				LEFT JOIN [Notification].[Exporter] E ON N.Id = E.NotificationId
			WHERE N.Id = @NotificationId
			RETURN
		END 
		
	BEGIN TRAN
		--Notifier
		UPDATE [Notification].[Exporter]
		SET FullName = 'Archived', Telephone = '000000', Fax = '000000', Email = 'archived@archive.com'
		WHERE NotificationId = @NotificationId
		UPDATE [Notification].[Exporter] 
		SET [Name] = 'Archived', Address1 = 'Archived', Address2 = 'Archived', TownOrCity = 'Archived', PostalCode = 'Archived', Region = 'Archived'
		WHERE NotificationId = @NotificationId AND [Type] in (2,3)

		UPDATE [Notification].[Producer]
		SET FullName = 'Archived', Telephone = '000000', Fax = '000000', Email = 'archived@archive.com'
		WHERE ProducerCollectionId IN (Select Id from [Notification].[ProducerCollection] where NotificationId = @NotificationId)
		UPDATE [Notification].[Producer]
		SET [Name] = 'Archived', Address1 = 'Archived', Address2 = 'Archived', TownOrCity = 'Archived', PostalCode = 'Archived'
		WHERE ProducerCollectionId IN (Select Id from [Notification].[ProducerCollection] where NotificationId = @NotificationId)
		AND [Type] in (2,3)

		--Consignee
		UPDATE [Notification].[Importer]
		SET FullName = 'Archived', Telephone = '000000', Fax = '000000', Email = 'archived@archive.com'
		WHERE NotificationId = @NotificationId
		UPDATE [Notification].[Importer]
		SET [Name] = 'Archived', Address1 = 'Archived', Address2 = 'Archived', TownOrCity = 'Archived', PostalCode = 'Archived'
		WHERE NotificationId = @NotificationId AND [Type] in (2,3)
				
		UPDATE [Notification].[Facility]
		SET FullName = 'Archived', Telephone = '000000', Fax = '000000', Email = 'archived@archive.com'
		WHERE FacilityCollectionId IN (Select Id from [Notification].[FacilityCollection] where NotificationId = @NotificationId)
		UPDATE [Notification].[Facility]
		SET [Name] = 'Archived', Address1 = 'Archived', Address2 = 'Archived', TownOrCity = 'Archived', PostalCode = 'Archived'
		WHERE FacilityCollectionId IN (Select Id from [Notification].[FacilityCollection] where NotificationId = @NotificationId)
		AND [Type] in (2,3)

		UPDATE [Notification].[Carrier]
		SET FullName = 'Archived', Telephone = '000000', Fax = '000000', Email = 'archived@archive.com'
		WHERE CarrierCollectionId IN (Select Id from [Notification].[CarrierCollection] where NotificationId = @NotificationId)
		UPDATE [Notification].[Carrier]
		SET [Name] = 'Archived', Address1 = 'Archived', Address2 = 'Archived', TownOrCity = 'Archived', PostalCode = 'Archived'
		WHERE CarrierCollectionId IN (Select Id from [Notification].[CarrierCollection] where NotificationId = @NotificationId)
		AND [Type] in (2,3)

		DELETE FROM [FileStore].[File] where Id in (select ProcessOfGenerationId FROM [Notification].[AnnexCollection] Where NotificationId = @NotificationId)
		DELETE FROM [FileStore].[File] where Id in (select WasteCompositionId FROM [Notification].[AnnexCollection] Where NotificationId = @NotificationId)
		DELETE FROM [FileStore].[File] where Id in (select TechnologyEmployedId FROM [Notification].[AnnexCollection] Where NotificationId = @NotificationId)

		DELETE FROM [FileStore].[File] where Id in (select FileId FROM [Notification].[Movement] Where NotificationId = @NotificationId)
		DELETE FROM [FileStore].[File] where Id in (select FileId FROM [Notification].[MovementOperationReceipt] 
			Where MovementId in (select Id FROM [Notification].[Movement] Where NotificationId = @NotificationId))
		DELETE FROM [FileStore].[File] where Id in (select FileId FROM [Notification].[MovementPartialRejection]
			Where MovementId in (select Id FROM [Notification].[Movement] Where NotificationId = @NotificationId))
		DELETE FROM [FileStore].[File] where Id in (select FileId FROM [Notification].[MovementReceipt] 
			Where MovementId in (select Id FROM [Notification].[Movement] Where NotificationId = @NotificationId))
		DELETE FROM [FileStore].[File] where Id in (select FileId FROM [Notification].[MovementRejection]
			Where MovementId in (select Id FROM [Notification].[Movement] Where NotificationId = @NotificationId))

		update [Notification].[Notification] SET IsArchived = 1, ArchivedByUserId = @CurrentUserId, ArchivedDate = SYSDATETIMEOFFSET() where Id = @NotificationId
				
		SELECT
			N.Id,
			N.NotificationNumber,
			LNS.[Description] AS [Status],
			N.CreatedDate as DateGenerated,
			E.[Name] AS CompanyName
		FROM
			[Notification].[Notification] N
			INNER JOIN [Notification].[NotificationAssessment] NA ON N.Id = NA.NotificationApplicationId
			INNER JOIN [Lookup].NotificationStatus LNS on LNS.Id = NA.[Status]
			LEFT JOIN [Notification].[Exporter] E ON N.Id = E.NotificationId
		WHERE N.Id = @NotificationId
	END
	COMMIT
END
