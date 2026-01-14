IF OBJECT_ID('[Notification].[uspDeleteExportNotification]') IS NULL
    EXEC('CREATE PROCEDURE [Notification].[uspDeleteExportNotification] AS SET NOCOUNT ON;')
GO

ALTER PROCEDURE [Notification].[uspDeleteExportNotification] 
	@NotificationId UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [Notification].[MovementDateHistory]
		WHERE MovementId IN (SELECT [Id] FROM [Notification].[Movement] WHERE NotificationId = @NotificationId)

	DELETE FROM [Notification].[MovementCarrier]
		WHERE MovementId IN (SELECT [Id] FROM [Notification].[Movement] WHERE NotificationId = @NotificationId)

	DELETE FROM [Notification].[MovementPackagingInfo]
		WHERE MovementDetailsId IN (SELECT [Id] FROM [Notification].[MovementDetails] WHERE MovementId IN (SELECT [Id] FROM [Notification].[Movement] WHERE NotificationId = @NotificationId))

	DELETE FROM [Notification].[MovementDetails]
		WHERE MovementId IN (SELECT [Id] FROM [Notification].[Movement] WHERE NotificationId = @NotificationId)

	DELETE FROM [Notification].[MovementOperationReceipt]
		WHERE MovementId IN (SELECT [Id] FROM [Notification].[Movement] WHERE NotificationId = @NotificationId)

	DELETE FROM [Notification].[MovementReceipt]
		WHERE MovementId IN (SELECT [Id] FROM [Notification].[Movement] WHERE NotificationId = @NotificationId)

	DELETE FROM [Notification].[MovementRejection]
		WHERE MovementId IN (SELECT [Id] FROM [Notification].[Movement] WHERE NotificationId = @NotificationId)

	DELETE FROM [Notification].[MovementStatusChange]
		WHERE MovementId IN (SELECT [Id] FROM [Notification].[Movement] WHERE NotificationId = @NotificationId)

	DELETE FROM [Notification].[Movement]
		WHERE NotificationId = @NotificationId

	DELETE FROM [Notification].[AnnexCollection]
		WHERE NotificationId = @NotificationId

	DELETE FROM [Notification].[Carrier]
		WHERE CarrierCollectionId IN (SELECT [Id] FROM [Notification].[CarrierCollection] WHERE NotificationId = @NotificationId)

	DELETE FROM [Notification].[CarrierCollection]
		WHERE NotificationId = @NotificationId

	DELETE FROM [Notification].[Consent]
		WHERE NotificationApplicationId = @NotificationId

	DELETE FROM [Notification].[Consultation]
		WHERE NotificationId = @NotificationId

	DELETE FROM [Notification].[DisposalInfo]
		WHERE NotificationId = @NotificationId

	DELETE FROM [Notification].[Exporter]
		WHERE NotificationId = @NotificationId

	DELETE FROM [Notification].[Facility]
		WHERE FacilityCollectionId IN (SELECT [Id] FROM [Notification].[FacilityCollection] WHERE NotificationId = @NotificationId)

	DELETE FROM [Notification].[FacilityCollection]
		WHERE NotificationId = @NotificationId

	DELETE FROM [Notification].[FinancialGuaranteeStatusChange]
		WHERE FinancialGuaranteeId IN (SELECT [Id] FROM [Notification].[FinancialGuarantee] WHERE FinancialGuaranteeCollectionId IN (SELECT [Id] FROM [Notification].[FinancialGuaranteeCollection] WHERE NotificationId = @NotificationId))

	DELETE FROM [Notification].[FinancialGuarantee]
		WHERE FinancialGuaranteeCollectionId IN (SELECT [Id] FROM [Notification].[FinancialGuaranteeCollection] WHERE NotificationId = @NotificationId)

	DELETE FROM [Notification].[FinancialGuaranteeCollection]
		WHERE NotificationId = @NotificationId

	DELETE FROM [Notification].[Importer]
		WHERE NotificationId = @NotificationId

	DELETE FROM [Notification].[MeansOfTransport]
		WHERE NotificationId = @NotificationId

	DELETE FROM [Notification].[NotificationDates]
		WHERE NotificationAssessmentId IN (SELECT [Id] FROM [Notification].[NotificationAssessment] WHERE NotificationApplicationId = @NotificationId)

	DELETE FROM [Notification].[NotificationStatusChange]
		WHERE NotificationAssessmentId IN (SELECT [Id] FROM [Notification].[NotificationAssessment] WHERE NotificationApplicationId = @NotificationId)

	DELETE FROM [Notification].[NotificationAssessment]
		WHERE NotificationApplicationId = @NotificationId

	DELETE FROM [Notification].[NumberOfShipmentsHistory]
		WHERE NotificationId = @NotificationId

	DELETE FROM [Notification].[OperationCodes]
		WHERE NotificationId = @NotificationId

	DELETE FROM [Notification].[PackagingInfo]
		WHERE NotificationId = @NotificationId

	DELETE FROM [Notification].[PhysicalCharacteristicsInfo]
		WHERE NotificationId = @NotificationId

	DELETE FROM [Notification].[Producer]
		WHERE ProducerCollectionId IN (SELECT [Id] FROM [Notification].[ProducerCollection] WHERE NotificationId = @NotificationId)

	DELETE FROM [Notification].[ProducerCollection]
		WHERE NotificationId = @NotificationId

	DELETE FROM [Notification].[RecoveryInfo]
		WHERE NotificationId = @NotificationId

	DELETE FROM [Notification].[ShipmentInfo]
		WHERE NotificationId = @NotificationId

	DELETE FROM [Notification].[TechnologyEmployed]
		WHERE NotificationId = @NotificationId

	DELETE FROM [Notification].[AdditionalCharges]
		WHERE NotificationId = @NotificationId

	DELETE FROM [Notification].[Transaction]
		WHERE NotificationId = @NotificationId

	DELETE FROM [Notification].[EntryCustomsOffice]
		WHERE TransportRouteId IN (SELECT [Id] FROM [Notification].[TransportRoute] WHERE NotificationId = @NotificationId)

	DELETE FROM [Notification].[ExitCustomsOffice]
		WHERE TransportRouteId IN (SELECT [Id] FROM [Notification].[TransportRoute] WHERE NotificationId = @NotificationId)

	DELETE FROM [Notification].[EntryExitCustomsSelection]
		WHERE TransportRouteId IN (SELECT [Id] FROM [Notification].[TransportRoute] WHERE NotificationId = @NotificationId)

	DELETE FROM [Notification].[StateOfExport]
		WHERE TransportRouteId IN (SELECT [Id] FROM [Notification].[TransportRoute] WHERE NotificationId = @NotificationId)

	DELETE FROM [Notification].[StateOfImport]
		WHERE TransportRouteId IN (SELECT [Id] FROM [Notification].[TransportRoute] WHERE NotificationId = @NotificationId)

	DELETE FROM [Notification].[TransitState]
		WHERE TransportRouteId IN (SELECT [Id] FROM [Notification].[TransportRoute] WHERE NotificationId = @NotificationId)

	DELETE FROM [Notification].[TransportRoute]
		WHERE NotificationId = @NotificationId

	DELETE FROM [Notification].[UserHistory]
		WHERE NotificationId = @NotificationId

	DELETE FROM [Notification].[WasteCodeInfo]
		WHERE NotificationId = @NotificationId

	DELETE FROM [Notification].[WasteComponentInfo]
		WHERE NotificationId = @NotificationId

	DELETE FROM [Notification].[WasteAdditionalInformation]
		WHERE WasteTypeId IN (SELECT [Id] FROM [Notification].[WasteType] WHERE NotificationId = @NotificationId)

	DELETE FROM [Notification].[WasteComposition]
		WHERE WasteTypeId IN (SELECT [Id] FROM [Notification].[WasteType] WHERE NotificationId = @NotificationId)

	DELETE FROM [Notification].[WasteType]
		WHERE NotificationId = @NotificationId

	DELETE FROM [Notification].[SharedUser]
		WHERE NotificationId = @NotificationId

	DELETE FROM [Notification].[SharedUserHistory]
		WHERE NotificationId = @NotificationId

	DELETE FROM [Notification].[Audit]
		WHERE NotificationId = @NotificationId

	DELETE FROM [Notification].[MovementAudit]
		WHERE NotificationId = @NotificationId

	DELETE FROM [Notification].[Comments]
		WHERE NotificationId = @NotificationId

	DELETE FROM [Notification].[Notification]
		WHERE Id = @NotificationId
END
GO