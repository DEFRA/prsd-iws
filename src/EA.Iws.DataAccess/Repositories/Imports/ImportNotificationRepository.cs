namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain.ImportNotification;
    using Domain.Security;
    using Prsd.Core;

    internal class ImportNotificationRepository : IImportNotificationRepository
    {
        private readonly ImportNotificationContext context;
        private readonly IImportNotificationApplicationAuthorization authorization;

        public ImportNotificationRepository(ImportNotificationContext context, IImportNotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public async Task<bool> NotificationNumberExists(string number)
        {
            Guard.ArgumentNotNull(() => number, number);

            return await context.ImportNotifications.AnyAsync(n => n.NotificationNumber == number);
        }

        public async Task<ImportNotification> Get(Guid id)
        {
            await authorization.EnsureAccessAsync(id);
            return await context.ImportNotifications.SingleAsync(n => n.Id == id);
        }

        public async Task Add(ImportNotification notification)
        {
            if (await context.ImportNotifications
                .AnyAsync(n => n.NotificationNumber == notification.NotificationNumber))
            {
                throw new InvalidOperationException("Cannot add an import notification with the duplicate number: " + notification.NotificationNumber);
            }

            context.ImportNotifications.Add(notification);
        }

        public async Task<NotificationType> GetTypeById(Guid id)
        {
            await authorization.EnsureAccessAsync(id);
            return (await context.ImportNotifications.SingleAsync(n => n.Id == id)).NotificationType;
        }

        public async Task<Guid?> GetIdOrDefault(string number)
        {
            return await context.ImportNotifications.Where(n => n.NotificationNumber == number)
                                                    .Select(n => (Guid?)n.Id)
                                                    .SingleOrDefaultAsync();
        }

        public async Task<bool> Delete(Guid notificationId)
        {
            var rowsAffected = await context.Database.ExecuteSqlCommandAsync(@"
                DELETE FROM [ImportNotification].[MovementOperationReceipt] WHERE MovementId IN (SELECT [Id] FROM [ImportNotification].[Movement] WHERE NotificationId = @Id)
                DELETE FROM [ImportNotification].[MovementReceipt] WHERE MovementId IN (SELECT [Id] FROM [ImportNotification].[Movement] WHERE NotificationId = @Id)
                DELETE FROM [ImportNotification].[MovementRejection] WHERE MovementId IN (SELECT [Id] FROM [ImportNotification].[Movement] WHERE NotificationId = @Id)
                DELETE FROM [ImportNotification].[Movement] WHERE NotificationId = @Id
                DELETE FROM [ImportNotification].[Consent] WHERE NotificationId = @Id
                DELETE FROM [ImportNotification].[Consultation] WHERE NotificationId = @Id
                DELETE FROM [ImportNotification].[Exporter] WHERE ImportNotificationId = @Id
                DELETE FROM [ImportNotification].[Facility] WHERE FacilityCollectionId IN (SELECT [Id] FROM [ImportNotification].[FacilityCollection] WHERE ImportNotificationId = @Id)
                DELETE FROM [ImportNotification].[FacilityCollection] WHERE ImportNotificationId = @Id
                DELETE FROM [ImportNotification].[FinancialGuaranteeStatusChange] WHERE FinancialGuaranteeId IN (SELECT [Id] FROM [ImportNotification].[FinancialGuarantee] WHERE ImportNotificationId  = @Id)
                DELETE FROM [ImportNotification].[FinancialGuarantee] WHERE ImportNotificationId = @Id
                DELETE FROM [ImportNotification].[FinancialGuaranteeApproval] WHERE ImportNotificationId = @Id
                DELETE FROM [ImportNotification].[FinancialGuaranteeRefusal] WHERE ImportNotificationId = @Id
                DELETE FROM [ImportNotification].[FinancialGuaranteeRelease] WHERE ImportNotificationId = @Id
                DELETE FROM [ImportNotification].[Importer] WHERE ImportNotificationId = @Id
                DELETE FROM [ImportNotification].[InterimStatus] WHERE ImportNotificationId = @Id
                DELETE FROM [ImportNotification].[NotificationDates] WHERE NotificationAssessmentId IN (SELECT [Id] FROM [ImportNotification].[NotificationAssessment] WHERE NotificationApplicationId = @Id)
                DELETE FROM [ImportNotification].[NotificationStatusChange] WHERE NotificationAssessmentId IN (SELECT [Id] FROM [ImportNotification].[NotificationAssessment] WHERE NotificationApplicationId = @Id)
                DELETE FROM [ImportNotification].[NotificationAssessment] WHERE NotificationApplicationId = @Id
                DELETE FROM [ImportNotification].[NumberOfShipmentsHistory] WHERE ImportNotificationId = @Id
                DELETE FROM [ImportNotification].[Objection] WHERE NotificationId = @Id
                DELETE FROM [ImportNotification].[Producer] WHERE ImportNotificationId = @Id
                DELETE FROM [ImportNotification].[Shipment] WHERE ImportNotificationId = @Id
                DELETE FROM [ImportNotification].[Transaction] WHERE NotificationId = @Id
                DELETE FROM [ImportNotification].[StateOfExport] WHERE TransportRouteId IN (SELECT [Id] FROM [ImportNotification].[TransportRoute] WHERE ImportNotificationId = @Id)
                DELETE FROM [ImportNotification].[StateOfImport] WHERE TransportRouteId IN (SELECT [Id] FROM [ImportNotification].[TransportRoute] WHERE ImportNotificationId = @Id)
                DELETE FROM [ImportNotification].[TransitState] WHERE TransportRouteId IN (SELECT [Id] FROM [ImportNotification].[TransportRoute] WHERE ImportNotificationId = @Id)
                DELETE FROM [ImportNotification].[TransportRoute] WHERE ImportNotificationId = @Id
                DELETE FROM [ImportNotification].[OperationCodes] WHERE WasteOperationId IN (SELECT [Id] FROM [ImportNotification].[WasteOperation] WHERE ImportNotificationId = @Id)
                DELETE FROM [ImportNotification].[WasteOperation] WHERE ImportNotificationId = @Id
                DELETE FROM [ImportNotification].[WasteCode] WHERE WasteTypeId IN (SELECT [Id] FROM [ImportNotification].[WasteType] WHERE ImportNotificationId = @Id)
                DELETE FROM [ImportNotification].[WasteType] WHERE ImportNotificationId = @Id
                DELETE FROM [ImportNotification].[Withdrawn] WHERE NotificationId = @Id
                DELETE FROM [ImportNotification].[Notification] WHERE Id = @Id",
                new SqlParameter("@Id", notificationId));

            return rowsAffected > 0;
        }
    }
}
