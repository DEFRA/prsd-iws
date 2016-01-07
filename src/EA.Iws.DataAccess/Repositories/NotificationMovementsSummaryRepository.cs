namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
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
                .Join(
                    context.FinancialGuarantees,
                    x => x.Notification.Id,
                    fg => fg.NotificationApplicationId,
                    (x, fg) => new { x.Notification, x.Shipment, FinancialGuarantee = fg })
                .Select(x => new
                {
                    NotificationId = x.Notification.Id,
                    x.Notification.NotificationType,
                    x.Notification.NotificationNumber,
                    x.FinancialGuarantee.ActiveLoadsPermitted,
                    NumberOfShipments = x.Shipment == null ? 0 : x.Shipment.NumberOfShipments,
                    Quantity = x.Shipment == null ? 0 : x.Shipment.Quantity,
                    Units = x.Shipment == null ? ShipmentQuantityUnits.Tonnes : x.Shipment.Units
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

            return NotificationMovementsSummary.Load(summaryData.NotificationId,
                summaryData.NotificationNumber,
                summaryData.NotificationType,
                summaryData.NumberOfShipments,
                totalMovements,
                summaryData.ActiveLoadsPermitted.GetValueOrDefault(),
                currentActiveLoads,
                summaryData.Quantity,
                (await quantity.Received(notificationId)).Quantity,
                summaryData.Units);
        }
    }
}