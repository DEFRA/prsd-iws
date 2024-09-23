namespace EA.Iws.RequestHandlers.ImportNotification
{
    using Core.ImportNotificationAssessment;
    using DataAccess;
    using Domain.ImportNotification;
    using EA.Iws.Domain.ImportNotificationAssessment;
    using EA.Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using System;
    using System.Threading.Tasks;

    internal class SetNewNumberOfShipmentsHandler : IRequestHandler<SetNewNumberOfShipments, bool>
    {
        private readonly ImportNotificationContext context;
        private readonly INumberOfShipmentsHistotyRepository shipmentHistotyRepository;
        private readonly IShipmentRepository shipmentRepository;
        private readonly IImportNotificationAssessmentRepository importNotificationAssessmentRepository;
        private readonly IUserContext userContext;

        public SetNewNumberOfShipmentsHandler(ImportNotificationContext context,
                                              INumberOfShipmentsHistotyRepository shipmentHistotyRepository,
                                              IShipmentRepository shipmentRepository,
                                              IImportNotificationAssessmentRepository importNotificationAssessmentRepository,
                                              IUserContext userContext)
        {
            this.context = context;
            this.shipmentHistotyRepository = shipmentHistotyRepository;
            this.shipmentRepository = shipmentRepository;
            this.importNotificationAssessmentRepository = importNotificationAssessmentRepository;
            this.userContext = userContext;
        }

        public async Task<bool> HandleAsync(SetNewNumberOfShipments message)
        {
            var shipment = await shipmentRepository.GetByNotificationId(message.NotificationId);
            shipment.UpdateNumberOfShipments(message.NewNumberOfShipments);

            shipmentHistotyRepository.Add(new NumberOfShipmentsHistory(message.NotificationId, message.OldNumberOfShipments, DateTime.UtcNow));

            var notificationAssesmentInfo = await importNotificationAssessmentRepository.GetByNotification(message.NotificationId);
            notificationAssesmentInfo.AddStatusChangeRecord(new ImportNotificationStatusChange(notificationAssesmentInfo.Status,
                                                                                               ImportNotificationStatus.Resubmitted,
                                                                                               userContext.UserId));
            await context.SaveChangesAsync();

            return true;
        }
    }
}
