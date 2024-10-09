namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication.Shipment;
    using EA.Iws.Domain.NotificationAssessment;
    using EA.Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class SetNewNumberOfShipmentsHandler : IRequestHandler<SetNewNumberOfShipments, bool>
    {
        private readonly IwsContext context;
        private readonly INumberOfShipmentsHistotyRepository shipmentHistotyRepository;
        private readonly IShipmentInfoRepository shipmentInfoRepository;
        private readonly INotificationAssessmentRepository notificationAssessmentRepository;
        private readonly IUserContext userContext;

        public SetNewNumberOfShipmentsHandler(IwsContext context, 
                                              INumberOfShipmentsHistotyRepository shipmentHistotyRepository, 
                                              IShipmentInfoRepository shipmentInfoRepository,
                                              INotificationAssessmentRepository notificationAssessmentRepository,
                                              IUserContext userContext)
        {
            this.context = context;
            this.shipmentHistotyRepository = shipmentHistotyRepository;
            this.shipmentInfoRepository = shipmentInfoRepository;
            this.notificationAssessmentRepository = notificationAssessmentRepository;
            this.userContext = userContext;
        }

        public async Task<bool> HandleAsync(SetNewNumberOfShipments message)
        {
            var shipmentInfo = await shipmentInfoRepository.GetByNotificationId(message.NotificationId);
            shipmentInfo.UpdateNumberOfShipments(message.NewNumberOfShipments);
            
            shipmentHistotyRepository.Add(new NumberOfShipmentsHistory(message.NotificationId, message.OldNumberOfShipments, DateTime.UtcNow));

            var notificationAssesmentInfo = await notificationAssessmentRepository.GetByNotificationId(message.NotificationId);
            var user = await context.Users.SingleAsync(u => u.Id == userContext.UserId.ToString());

            if (message.OldNumberOfShipments != message.NewNumberOfShipments)
            {
                notificationAssesmentInfo.AddStatusChangeRecord(new NotificationStatusChange(Core.NotificationAssessment.NotificationStatus.Resubmitted, user));
            }

            await context.SaveChangesAsync();
            
            return true;
        }
    }
}
