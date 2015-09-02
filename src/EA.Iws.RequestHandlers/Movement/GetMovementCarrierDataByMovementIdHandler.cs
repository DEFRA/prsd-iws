namespace EA.Iws.RequestHandlers.Movement
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Carriers;
    using DataAccess;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class GetMovementCarrierDataByMovementIdHandler : IRequestHandler<GetMovementCarrierDataByMovementId, MovementCarrierData>
    {
        private readonly IwsContext context;
        private readonly IMap<NotificationApplication, IList<CarrierData>> notificationCarrierMapper;
        private readonly IMap<Movement, Dictionary<int, CarrierData>> movementCarrierMapper;

        public GetMovementCarrierDataByMovementIdHandler(IwsContext context,
            IMap<NotificationApplication, IList<CarrierData>> notificationCarrierMapper,
            IMap<Movement, Dictionary<int, CarrierData>> movementCarrierMapper)
        {
            this.context = context;
            this.notificationCarrierMapper = notificationCarrierMapper;
            this.movementCarrierMapper = movementCarrierMapper;
        }

        public async Task<MovementCarrierData> HandleAsync(GetMovementCarrierDataByMovementId message)
        {
            var movement = await context.Movements.Where(m => m.Id == message.MovementId).SingleAsync();

            return new MovementCarrierData
            {
                NotificationCarriers = notificationCarrierMapper.Map(movement.NotificationApplication),
                SelectedCarriers = movementCarrierMapper.Map(movement)
            };
        }
    }
}
