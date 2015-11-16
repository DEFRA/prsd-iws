namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using NotificationApplication.Shipment;
    using NotificationAssessment;

    public class MovementFactory
    {
        private readonly MovementNumberGenerator numberGenerator;
        private readonly INotificationAssessmentRepository assessmentRepository;
        private readonly IMovementRepository movementRepository;
        private readonly IShipmentInfoRepository shipmentRepository;

        public MovementFactory(IShipmentInfoRepository shipmentRepository,
            IMovementRepository movementRepository,
            INotificationAssessmentRepository assessmentRepository,
            MovementNumberGenerator numberGenerator)
        {
            this.shipmentRepository = shipmentRepository;
            this.movementRepository = movementRepository;
            this.assessmentRepository = assessmentRepository;
            this.numberGenerator = numberGenerator;
        }

        public async Task<Movement> Create(Guid notificationId, DateTime actualMovementDate)
        {
            var maxNumberOfShipments = (await shipmentRepository.GetByNotificationId(notificationId)).NumberOfShipments;
            var currentNumberOfShipments = (await movementRepository.GetAllMovements(notificationId)).Count();

            if (maxNumberOfShipments == currentNumberOfShipments)
            {
                throw new InvalidOperationException(
                    string.Format("Cannot create new movement for notification {0} which has used {1} of {2} movements",
                        notificationId, currentNumberOfShipments, maxNumberOfShipments));
            }

            var notificationStatus = (await assessmentRepository.GetByNotificationId(notificationId)).Status;

            if (notificationStatus != NotificationStatus.Consented)
            {
                throw new InvalidOperationException(
                    string.Format("Cannot create a movement for notification {0} because it's status is {1}",
                        notificationId, notificationStatus));
            }

            var newNumber = await numberGenerator.Generate(notificationId);

            return new Movement(newNumber, notificationId, actualMovementDate);
        }
    }
}