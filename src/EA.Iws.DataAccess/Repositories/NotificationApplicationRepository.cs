namespace EA.Iws.DataAccess.Repositories
{
    using Core.Notification;
    using Core.Shared;
    using Domain.NotificationApplication;
    using Domain.Security;
    using EA.Iws.Core.Extensions;
    using EA.Iws.Core.NotificationAssessment;
    using EA.Prsd.Core.Helpers;
    using System;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

    internal class NotificationApplicationRepository : INotificationApplicationRepository
    {
        private readonly IwsContext context;
        private readonly INotificationApplicationAuthorization notificationApplicationAuthorization;

        public NotificationApplicationRepository(IwsContext context,
            INotificationApplicationAuthorization notificationApplicationAuthorization)
        {
            this.context = context;
            this.notificationApplicationAuthorization = notificationApplicationAuthorization;
        }

        public async Task<NotificationApplication> GetById(Guid id)
        {
            await notificationApplicationAuthorization.EnsureAccessAsync(id);
            return await context.NotificationApplications.SingleAsync(n => n.Id == id);
        }

        public async Task<NotificationApplication> GetByMovementId(Guid movementId)
        {
            var notificationId = await context.Movements
                .Where(m => m.Id == movementId)
                .Select(m => m.NotificationId)
                .SingleAsync();

            return await GetById(notificationId);
        }

        public async Task<string> GetNumber(Guid id)
        {
            return await context.NotificationApplications
                .Where(n => n.Id == id)
                .Select(n => n.NotificationNumber)
                .SingleAsync();
        }

        public async Task<Guid?> GetIdOrDefault(string number, bool isDeleteNotification)
        {
            if (isDeleteNotification)
            {
                return await context.NotificationApplications
                    .Where(n => number.Replace(" ", string.Empty) == n.NotificationNumber.Replace(" ", string.Empty))
                    .Select(n => (Guid?)n.Id)
                    .SingleOrDefaultAsync();
            }
            else
            {
                return await context.NotificationApplications
                    .Where(n => number.Replace(" ", string.Empty) == n.NotificationNumber.Replace(" ", string.Empty))
                    .Select(n => (Guid?)n.Id)
                    .SingleOrDefaultAsync();
            }
        }

        public async Task<bool> GetIsArchived(Guid id)
        {
            return await context.NotificationApplications
                .Where(n => n.Id == id)
                .Select(n => n.IsArchived)
                .SingleOrDefaultAsync();
        }

        public void Add(NotificationApplication notification)
        {
            context.NotificationApplications.Add(notification);
        }

        public async Task<bool> NotificationNumberExists(int number, UKCompetentAuthority competentAuthority)
        {
            var formattedNumber = NotificationNumberFormatter.GetNumber(number, competentAuthority);

            return await context.NotificationApplications.AnyAsync(n => n.NotificationNumber == formattedNumber);
        }

        public async Task<NotificationType> GetNotificationType(Guid id)
        {
            var notification = await GetById(id);

            return notification.NotificationType;
        }

        public async Task<UKCompetentAuthority> GetNotificationCompetentAuthority(Guid id)
        {
            var notification = await GetById(id);

            return notification.CompetentAuthority;
        }

        public async Task<bool> Delete(Guid notificationId)
        {
            var rowsAffected = await context.Database.ExecuteSqlCommandAsync(@"
                DELETE FROM [Notification].[MovementDateHistory] WHERE MovementId IN (SELECT [Id] FROM [Notification].[Movement] WHERE NotificationId = @Id)
                DELETE FROM [Notification].[MovementCarrier] WHERE MovementId IN (SELECT [Id] FROM [Notification].[Movement] WHERE NotificationId = @Id)
                DELETE FROM [Notification].[MovementPackagingInfo] WHERE MovementDetailsId IN (SELECT [Id] FROM [Notification].[MovementDetails] WHERE MovementId IN (SELECT [Id] FROM [Notification].[Movement] WHERE NotificationId = @Id))
                DELETE FROM [Notification].[MovementDetails] WHERE MovementId IN (SELECT [Id] FROM [Notification].[Movement] WHERE NotificationId = @Id)
                DELETE FROM [Notification].[MovementOperationReceipt] WHERE MovementId IN (SELECT [Id] FROM [Notification].[Movement] WHERE NotificationId = @Id)
                DELETE FROM [Notification].[MovementReceipt] WHERE MovementId IN (SELECT [Id] FROM [Notification].[Movement] WHERE NotificationId = @Id)
                DELETE FROM [Notification].[MovementRejection] WHERE MovementId IN (SELECT [Id] FROM [Notification].[Movement] WHERE NotificationId = @Id)
                DELETE FROM [Notification].[MovementStatusChange] WHERE MovementId IN (SELECT [Id] FROM [Notification].[Movement] WHERE NotificationId = @Id)
                DELETE FROM [Notification].[Movement] WHERE NotificationId = @Id
                DELETE FROM [Notification].[AnnexCollection] WHERE NotificationId = @Id
                DELETE FROM [Notification].[Carrier] WHERE CarrierCollectionId IN (SELECT [Id] FROM [Notification].[CarrierCollection] WHERE NotificationId = @Id)
                DELETE FROM [Notification].[CarrierCollection] WHERE NotificationId = @Id
                DELETE FROM [Notification].[Consent] WHERE NotificationApplicationId = @Id
                DELETE FROM [Notification].[Consultation] WHERE NotificationId = @Id
                DELETE FROM [Notification].[DisposalInfo] WHERE NotificationId = @Id
                DELETE FROM [Notification].[Exporter] WHERE NotificationId = @Id
                DELETE FROM [Notification].[Facility] WHERE FacilityCollectionId IN (SELECT [Id] FROM [Notification].[FacilityCollection] WHERE NotificationId = @Id)
                DELETE FROM [Notification].[FacilityCollection] WHERE NotificationId = @Id
                DELETE FROM [Notification].[FinancialGuaranteeStatusChange] WHERE FinancialGuaranteeId IN (SELECT [Id] FROM [Notification].[FinancialGuarantee] WHERE FinancialGuaranteeCollectionId IN (SELECT [Id] FROM [Notification].[FinancialGuaranteeCollection] WHERE NotificationId = @Id))
                DELETE FROM [Notification].[FinancialGuarantee] WHERE FinancialGuaranteeCollectionId IN (SELECT [Id] FROM [Notification].[FinancialGuaranteeCollection] WHERE NotificationId = @Id)
                DELETE FROM [Notification].[FinancialGuaranteeCollection] WHERE NotificationId = @Id
                DELETE FROM [Notification].[Importer] WHERE NotificationId = @Id
                DELETE FROM [Notification].[MeansOfTransport] WHERE NotificationId = @Id
                DELETE FROM [Notification].[NotificationDates] WHERE NotificationAssessmentId IN (SELECT [Id] FROM [Notification].[NotificationAssessment] WHERE NotificationApplicationId = @Id)
                DELETE FROM [Notification].[NotificationStatusChange] WHERE NotificationAssessmentId IN (SELECT [Id] FROM [Notification].[NotificationAssessment] WHERE NotificationApplicationId = @Id)
                DELETE FROM [Notification].[NotificationAssessment] WHERE NotificationApplicationId = @Id
                DELETE FROM [Notification].[NumberOfShipmentsHistory] WHERE NotificationId = @Id
                DELETE FROM [Notification].[OperationCodes] WHERE NotificationId = @Id
                DELETE FROM [Notification].[PackagingInfo] WHERE NotificationId = @Id
                DELETE FROM [Notification].[PhysicalCharacteristicsInfo] WHERE NotificationId = @Id
                DELETE FROM [Notification].[Producer] WHERE ProducerCollectionId IN (SELECT [Id] FROM [Notification].[ProducerCollection] WHERE NotificationId = @Id)
                DELETE FROM [Notification].[ProducerCollection] WHERE NotificationId = @Id
                DELETE FROM [Notification].[RecoveryInfo] WHERE NotificationId = @Id
                DELETE FROM [Notification].[ShipmentInfo] WHERE NotificationId = @Id
                DELETE FROM [Notification].[TechnologyEmployed] WHERE NotificationId = @Id
                DELETE FROM [Notification].[AdditionalCharges] WHERE NotificationId = @Id
                DELETE FROM [Notification].[Transaction] WHERE NotificationId = @Id
                DELETE FROM [Notification].[EntryCustomsOffice] WHERE TransportRouteId IN (SELECT [Id] FROM [Notification].[TransportRoute] WHERE NotificationId = @Id)
                DELETE FROM [Notification].[ExitCustomsOffice] WHERE TransportRouteId IN (SELECT [Id] FROM [Notification].[TransportRoute] WHERE NotificationId = @Id)
                DELETE FROM [Notification].[EntryExitCustomsSelection] WHERE TransportRouteId IN (SELECT [Id] FROM [Notification].[TransportRoute] WHERE NotificationId = @Id)
                DELETE FROM [Notification].[StateOfExport] WHERE TransportRouteId IN (SELECT [Id] FROM [Notification].[TransportRoute] WHERE NotificationId = @Id)
                DELETE FROM [Notification].[StateOfImport] WHERE TransportRouteId IN (SELECT [Id] FROM [Notification].[TransportRoute] WHERE NotificationId = @Id)
                DELETE FROM [Notification].[TransitState] WHERE TransportRouteId IN (SELECT [Id] FROM [Notification].[TransportRoute] WHERE NotificationId = @Id)
                DELETE FROM [Notification].[TransportRoute] WHERE NotificationId = @Id
                DELETE FROM [Notification].[UserHistory] WHERE NotificationId = @Id
                DELETE FROM [Notification].[WasteCodeInfo] WHERE NotificationId = @Id
                DELETE FROM [Notification].[WasteComponentInfo] WHERE NotificationId = @Id
                DELETE FROM [Notification].[WasteAdditionalInformation] WHERE WasteTypeId IN (SELECT [Id] FROM [Notification].[WasteType] WHERE NotificationId = @Id)
                DELETE FROM [Notification].[WasteComposition] WHERE WasteTypeId IN (SELECT [Id] FROM [Notification].[WasteType] WHERE NotificationId = @Id)
                DELETE FROM [Notification].[WasteType] WHERE NotificationId = @Id
                DELETE FROM [Notification].[SharedUser] WHERE NotificationId = @Id
                DELETE FROM [Notification].[SharedUserHistory] WHERE NotificationId = @Id
                DELETE FROM [Notification].[Audit] WHERE NotificationId = @Id
                DELETE FROM [Notification].[MovementAudit] WHERE NotificationId = @Id
                DELETE FROM [Notification].[Comments] WHERE NotificationId = @Id
                DELETE FROM [Notification].[Notification] WHERE Id = @Id",
                new SqlParameter("@Id", notificationId));

            return rowsAffected > 0;
        }

        public async Task<bool> DeleteExportNotification(Guid notificationId)
        {
            await notificationApplicationAuthorization.EnsureAccessAsync(notificationId);

            NotificationStatus notificationStatus = await context.NotificationAssessments.Where(n => n.NotificationApplicationId == notificationId)
                                                                                         .Select(n => n.Status)
                                                                                         .SingleOrDefaultAsync();

            if (notificationStatus.Equals(NotificationStatus.NotSubmitted))
            {
                var rowsAffected = await context.Database.ExecuteSqlCommandAsync(@"EXEC [Notification].[uspDeleteExportNotification] @NotificationId",
                                                                             new SqlParameter("@NotificationId", notificationId));

                return rowsAffected > 0;
            }

            return false;
        }

        public async Task<DeleteExportNotificationDetails> ValidateExportNotification(string exportNotificationNumber)
        {
            var notificationId = await context.NotificationApplications
                    .Where(n => exportNotificationNumber.Replace(" ", string.Empty) == n.NotificationNumber.Replace(" ", string.Empty))
                    .Select(n => (Guid?)n.Id)
                    .SingleOrDefaultAsync();

            try
            {
                await notificationApplicationAuthorization.EnsureAccessAsync(notificationId.GetValueOrDefault());
            }
            catch (Exception)
            {
                return new DeleteExportNotificationDetails()
                {
                    IsNotificationCanDeleted = false,
                    ErrorMessage = "You don't have permission to delete this notification."
                };
            }

            NotificationStatus notificationStatus = await context.NotificationAssessments.Where(n => n.NotificationApplicationId == notificationId)
                                                                                         .Select(n => n.Status)
                                                                                         .SingleOrDefaultAsync();

            if (!notificationStatus.Equals(NotificationStatus.NotSubmitted))
            {
                return new DeleteExportNotificationDetails()
                {
                    IsNotificationCanDeleted = false,
                    ErrorMessage = "You don't have permission to delete the notification status as a " + EnumHelper.GetDisplayName(notificationStatus).ToLower()
                };
            }

            return new DeleteExportNotificationDetails()
            {
                IsNotificationCanDeleted = true,
                ErrorMessage = string.Empty,
                NotificationId = notificationId.GetValueOrDefault()
            };
        }
    }
}