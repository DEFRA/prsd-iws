namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.FinancialGuarantee;
    using Core.Movement;
    using Core.Shared;
    using Domain.Movement;
    using Domain.Security;
    using Prsd.Core;

    internal class NotificationMovementsSummaryRepository : INotificationMovementsSummaryRepository
    {
        private readonly IwsContext context;
        private readonly INotificationApplicationAuthorization notificationAuthorization;
        private readonly NotificationMovementsQuantity quantity;

        public NotificationMovementsSummaryRepository(INotificationApplicationAuthorization notificationAuthorization,
            NotificationMovementsQuantity quantity,
            IwsContext context)
        {
            this.quantity = quantity;
            this.notificationAuthorization = notificationAuthorization;
            this.context = context;
        }

        public async Task<NotificationMovementsSummary> GetById(Guid notificationId)
        {
            await notificationAuthorization.EnsureAccessAsync(notificationId);

            var summaryData = await context.NotificationApplications
                .GroupJoin(
                    context.ShipmentInfos,
                    notification => notification.Id,
                    shipment => shipment.NotificationId,
                    (notification, shipments) => new { Notification = notification, Shipment = shipments.FirstOrDefault() })
                .Join(context.NotificationAssessments,
                    x => x.Notification.Id,
                    na => na.NotificationApplicationId,
                    (x, na) => new { x.Notification, x.Shipment, NotificationAssessment = na })
                .Select(x => new
                {
                    NotificationId = x.Notification.Id,
                    x.Notification.NotificationType,
                    x.Notification.NotificationNumber,
                    NumberOfShipments = x.Shipment == null ? 0 : x.Shipment.NumberOfShipments,
                    Quantity = x.Shipment == null ? 0 : x.Shipment.Quantity,
                    Units = x.Shipment == null ? ShipmentQuantityUnits.Tonnes : x.Shipment.Units,
                    NotificationStatus = x.NotificationAssessment.Status,
                    x.Notification.CompetentAuthority
                })
                .SingleAsync(x => x.NotificationId == notificationId);

            var totalMovements = await context.Movements
                .Where(m => 
                    m.NotificationId == notificationId)
                .CountAsync();

            var currentActiveLoads = await context.Movements
                .Where(m => 
                    m.NotificationId == notificationId
                    && (m.Status == MovementStatus.Submitted 
                        || m.Status == MovementStatus.Received) 
                    && m.Date < SystemTime.UtcNow)
                .CountAsync();

            var financialGuaranteeCollection =
                await context.FinancialGuarantees.SingleAsync(x => x.NotificationId == notificationId);

            var financialGuarantee = financialGuaranteeCollection.GetCurrentApprovedFinancialGuarantee() ??
                                     financialGuaranteeCollection.GetLatestFinancialGuarantee();

            return NotificationMovementsSummary.Load(summaryData.NotificationId,
                summaryData.NotificationNumber,
                summaryData.NotificationType,
                summaryData.NumberOfShipments,
                totalMovements,
                financialGuarantee == null ? 0 : financialGuarantee.ActiveLoadsPermitted.GetValueOrDefault(),
                currentActiveLoads,
                summaryData.Quantity,
                (await quantity.Received(notificationId)).Quantity,
                summaryData.Units,
                financialGuarantee == null ? FinancialGuaranteeStatus.AwaitingApplication : financialGuarantee.Status,
                summaryData.CompetentAuthority,
                summaryData.NotificationStatus);
        }
    }
}