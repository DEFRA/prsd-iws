namespace EA.Iws.RequestHandlers.NotificationMovements.Create
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.FinancialGuarantee;
    using Core.Movement;
    using Domain.FinancialGuarantee;
    using Domain.Movement;
    using Domain.NotificationApplication.Shipment;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.Create;

    internal class GetRemainingShipmentsHandler : IRequestHandler<GetRemainingShipments, RemainingShipmentsData>
    {
        private readonly IMovementRepository movementRepository;
        private readonly IShipmentInfoRepository shipmentRepository;
        private readonly IFinancialGuaranteeRepository financialGuaranteeRepository;

        public GetRemainingShipmentsHandler(IMovementRepository movementRepository,
            IShipmentInfoRepository shipmentRepository,
            IFinancialGuaranteeRepository financialGuaranteeRepository)
        {
            this.movementRepository = movementRepository;
            this.shipmentRepository = shipmentRepository;
            this.financialGuaranteeRepository = financialGuaranteeRepository;
        }

        public async Task<RemainingShipmentsData> HandleAsync(GetRemainingShipments message)
        {
            var maxNumberOfShipments =
                (await shipmentRepository.GetByNotificationId(message.NotificationId)).NumberOfShipments;
            var currentNumberOfShipments = (await movementRepository.GetAllMovements(message.NotificationId)).Count();

            var financialGuaranteeCollection = await financialGuaranteeRepository.GetByNotificationId(message.NotificationId);

            var currentFinancialGuarantee =
                financialGuaranteeCollection.FinancialGuarantees.SingleOrDefault(
                    fg => fg.Status == FinancialGuaranteeStatus.Approved);

            var activeLoadsPermitted = currentFinancialGuarantee == null ? 0 : currentFinancialGuarantee.ActiveLoadsPermitted.GetValueOrDefault();

            var activeShipmentsByDate = message.ShipmentDate.HasValue
                ? (await movementRepository.GetAllActiveMovements(message.NotificationId)).Count(
                    m => m.Date.Date == message.ShipmentDate.Value.Date)
                : 0;

            var remainingShipments = maxNumberOfShipments - currentNumberOfShipments;
            var activeLoadsRemainingByDate = activeLoadsPermitted - activeShipmentsByDate;

            return new RemainingShipmentsData
            {
                ActiveLoadsPermitted = activeLoadsPermitted,
                ActiveLoadsRemainingByDate = activeLoadsRemainingByDate,
                ShipmentsRemaining = remainingShipments,
            };
        }
    }
}