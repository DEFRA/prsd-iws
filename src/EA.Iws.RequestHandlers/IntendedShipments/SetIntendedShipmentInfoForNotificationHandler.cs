namespace EA.Iws.RequestHandlers.IntendedShipments
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Shipment;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.IntendedShipments;

    internal class SetIntendedShipmentInfoForNotificationHandler :
        IRequestHandler<SetIntendedShipmentInfoForNotification, Guid>
    {
        private readonly IwsContext context;
        private readonly IFacilityRepository facilityRepository;
        private readonly INotificationAssessmentRepository assessmentRepository;
        private readonly IShipmentInfoRepository shipmentInfoRepository;

        public SetIntendedShipmentInfoForNotificationHandler(IwsContext context,
            IShipmentInfoRepository shipmentInfoRepository,
            IFacilityRepository facilityRepository,
            INotificationAssessmentRepository assessmentRepository)
        {
            this.context = context;
            this.shipmentInfoRepository = shipmentInfoRepository;
            this.facilityRepository = facilityRepository;
            this.assessmentRepository = assessmentRepository;
        }

        public async Task<Guid> HandleAsync(SetIntendedShipmentInfoForNotification command)
        {
            var facilityCollection = await facilityRepository.GetByNotificationId(command.NotificationId);
            var shipmentInfo = await shipmentInfoRepository.GetByNotificationId(command.NotificationId);
            var status = await assessmentRepository.GetStatusByNotificationId(command.NotificationId);

            var shipmentPeriod = new ShipmentPeriod(
                command.StartDate,
                command.EndDate,
                facilityCollection.AllFacilitiesPreconsented.GetValueOrDefault());

            var shipmentQuantity = new ShipmentQuantity(
                command.Quantity,
                command.Units);

            if (shipmentInfo == null)
            {
                shipmentInfo = new ShipmentInfo(command.NotificationId,
                    shipmentPeriod,
                    command.NumberOfShipments,
                    shipmentQuantity);

                context.ShipmentInfos.Add(shipmentInfo);
            }
            else
            {
                shipmentInfo.UpdateNumberOfShipments(command.NumberOfShipments);
                shipmentInfo.UpdateShipmentPeriod(shipmentPeriod, status);
                shipmentInfo.UpdateQuantity(shipmentQuantity);
            }

            await context.SaveChangesAsync();

            return shipmentInfo.Id;
        }
    }
}