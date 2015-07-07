namespace EA.Iws.DataAccess.Tests.Integration
{
    using System;
    using System.Data.Entity;

    public class DatabaseDataDeleter
    {
        private static string deleteCommand = @"
        DECLARE @NotificationId UNIQUEIDENTIFIER = '{0}'

        DELETE FROM [Business].[Exporter]
        WHERE NotificationId = @NotificationId;

        DELETE FROM [Business].[Importer]
        WHERE NotificationId = @NotificationId;

        DELETE FROM [Business].[Producer]
        WHERE NotificationId = @NotificationId;

        DELETE FROM [Business].[Carrier]
        WHERE NotificationId = @NotificationId;

        DELETE FROM [Business].[Facility]
        WHERE NotificationId = @NotificationId;

        DELETE FROM [Business].[OperationCodes]
        WHERE NotificationId = @NotificationId;

        DELETE FROM [Business].[PackagingInfo]
        WHERE NotificationId = @NotificationId;

        DELETE FROM [Business].[PhysicalCharacteristicsInfo]
        WHERE NotificationId = @NotificationId;

        DELETE FROM [Business].[RecoveryInfo]
        WHERE NotificationId = @NotificationId;

        DELETE FROM [Business].[ShipmentInfo]
        WHERE NotificationId = @NotificationId;

        DELETE FROM [Business].[WasteAdditionalInformation]
        WHERE WasteTypeId IN (SELECT [Id] FROM [Business].[WasteType] WHERE NotificationId = @NotificationId);

        DELETE FROM [Business].[WasteComposition]
        WHERE WasteTypeId IN (SELECT [Id] FROM [Business].[WasteType] WHERE NotificationId = @NotificationId);

        DELETE FROM [Business].[WasteType] 
        WHERE NotificationId = @NotificationId;

        DELETE FROM [Business].[WasteCodeInfo] 
        WHERE NotificationId = @NotificationId;

        DELETE FROM [Notification].[EntryCustomsOffice] 
        WHERE NotificationId = @NotificationId;

        DELETE FROM [Notification].[ExitCustomsOffice] 
        WHERE NotificationId = @NotificationId;

        DELETE FROM [Notification].[StateOfExport] 
        WHERE NotificationId = @NotificationId;

        DELETE FROM [Notification].[StateOfImport] 
        WHERE NotificationId = @NotificationId;

        DELETE FROM [Notification].[TechnologyEmployed] 
        WHERE NotificationId = @NotificationId;

        DELETE FROM [Notification].[TransitState] 
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
