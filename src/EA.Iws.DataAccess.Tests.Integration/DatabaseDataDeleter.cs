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

        DELETE FROM [Notification].[Producer]
        WHERE NotificationId = @NotificationId;

        DELETE FROM [Notification].[Carrier]
        WHERE NotificationId = @NotificationId;

        DELETE FROM [Notification].[Facility]
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

        DELETE FROM [Notification].[FinancialGuaranteeStatusChange]
        WHERE FinancialGuaranteeId = 
            (SELECT Id FROM [Notification].[FinancialGuarantee] WHERE NotificationApplicationId = @NotificationId);

        DELETE FROM [Notification].[FinancialGuarantee]
        WHERE NotificationApplicationId = @NotificationId;

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
