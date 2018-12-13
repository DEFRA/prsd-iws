namespace EA.Iws.DataAccess.Tests.Integration
{
    using System;
    using System.Data.Entity;

    public class DatabaseDataDeleter
    {
        private static string deleteCommand = @"
        DECLARE @NotificationId UNIQUEIDENTIFIER = '{0}'

        DELETE FROM [Notification].[Exporter]
        WHERE NotificationId = @NotificationId;

        DELETE FROM [Notification].[Importer]
        WHERE NotificationId = @NotificationId;

        DELETE P
		FROM [Notification].[Producer] P
		INNER JOIN [Notification].[ProducerCollection] PC ON PC.Id = P.ProducerCollectionId
		WHERE PC.NotificationId = @NotificationId;

		DELETE FROM [Notification].[ProducerCollection]
		WHERE NotificationId = @NotificationId;

        DELETE C
		FROM [Notification].[Carrier] C
		INNER JOIN [Notification].[CarrierCollection] CC ON CC.Id = C.CarrierCollectionId
		WHERE CC.NotificationId = @NotificationId;

		DELETE FROM [Notification].[CarrierCollection]
		WHERE NotificationId = @NotificationId;

        DELETE F
		FROM [Notification].[Facility] F
		INNER JOIN [Notification].[FacilityCollection] FC ON FC.Id = F.FacilityCollectionId
		WHERE FC.NotificationId = @NotificationId;

		DELETE FROM [Notification].[FacilityCollection]
		WHERE NotificationId = @NotificationId;

        DELETE FROM [Notification].[OperationCodes]
        WHERE NotificationId = @NotificationId;

        DELETE FROM [Notification].[PackagingInfo]
        WHERE NotificationId = @NotificationId;

        DELETE FROM [Notification].[PhysicalCharacteristicsInfo]
        WHERE NotificationId = @NotificationId;

        DELETE FROM [Notification].[RecoveryInfo]
        WHERE NotificationId = @NotificationId;

        DELETE FROM [Notification].[DisposalInfo]
        WHERE NotificationId = @NotificationId;

        DELETE FROM [Notification].[ShipmentInfo]
        WHERE NotificationId = @NotificationId;

        DELETE FROM [Notification].[WasteAdditionalInformation]
        WHERE WasteTypeId IN (SELECT [Id] FROM [Notification].[WasteType] WHERE NotificationId = @NotificationId);

        DELETE FROM [Notification].[WasteComposition]
        WHERE WasteTypeId IN (SELECT [Id] FROM [Notification].[WasteType] WHERE NotificationId = @NotificationId);

        DELETE FROM [Notification].[WasteType] 
        WHERE NotificationId = @NotificationId;

        DELETE FROM [Notification].[WasteCodeInfo] 
        WHERE NotificationId = @NotificationId;

		DECLARE @TransportRouteId UNIQUEIDENTIFIER = 
		(SELECT [Id] FROM [Notification].[TransportRoute] WHERE [NotificationId] = @NotificationId)

        DELETE FROM [Notification].[EntryCustomsOffice] 
        WHERE TransportRouteId = @TransportRouteId;

        DELETE FROM [Notification].[ExitCustomsOffice] 
        WHERE TransportRouteId = @TransportRouteId;

        DELETE FROM [Notification].[StateOfExport] 
        WHERE TransportRouteId = @TransportRouteId;

        DELETE FROM [Notification].[StateOfImport] 
        WHERE TransportRouteId = @TransportRouteId;

        DELETE FROM [Notification].[TransitState] 
        WHERE TransportRouteId = @TransportRouteId;

		DELETE FROM [Notification].[TransportRoute]
		WHERE Id = @TransportRouteId;

        DELETE FROM [Notification].[TechnologyEmployed] 
        WHERE NotificationId = @NotificationId;

        DELETE FROM [Notification].[NotificationDates]
        WHERE NotificationAssessmentId = 
            (SELECT Id FROM [Notification].[NotificationAssessment] WHERE NotificationApplicationId = @NotificationId);

        DELETE FROM [Notification].[NotificationAssessment]
        WHERE NotificationApplicationId = @NotificationId;

        DELETE FGSC
		FROM [Notification].[FinancialGuaranteeStatusChange] FGSC
			 INNER JOIN [Notification].[FinancialGuarantee] FG ON FGSC.FinancialGuaranteeId = FG.Id
			 INNER JOIN [Notification].[FinancialGuaranteeCollection] FGC ON FG.FinancialGuaranteeCollectionId = FGC.Id
		WHERE FGC.NotificationId = @NotificationId;

		DELETE FG
		FROM [Notification].[FinancialGuarantee] FG
			 INNER JOIN [Notification].[FinancialGuaranteeCollection] FGC ON FG.FinancialGuaranteeCollectionId = FGC.Id
		WHERE FGC.NotificationId = @NotificationId;

		DELETE FROM [Notification].[FinancialGuaranteeCollection]
		WHERE NotificationId = @NotificationId;

        DELETE FROM [Notification].[AnnexCollection] 
        WHERE NotificationId = @NotificationId;

        DELETE FROM [Notification].[Notification] 
        WHERE Id = @NotificationId;

        ";

        public static void DeleteDataForNotification(Guid notificationId, IwsContext context)
        {
            context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, string.Format(deleteCommand, notificationId));
        }
    }
}
