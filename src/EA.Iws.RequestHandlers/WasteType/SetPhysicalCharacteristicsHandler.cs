namespace EA.Iws.RequestHandlers.WasteType
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.WasteType;

    internal class SetPhysicalCharacteristicsHandler : IRequestHandler<SetPhysicalCharacteristics, Guid>
    {
        private readonly IwsContext db;
        private readonly IMap<IList<PhysicalCharacteristicType>, IList<Domain.Notification.PhysicalCharacteristicType>> mapper;

        public SetPhysicalCharacteristicsHandler(IwsContext db, IMap<IList<PhysicalCharacteristicType>, IList<Domain.Notification.PhysicalCharacteristicType>> mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<Guid> HandleAsync(SetPhysicalCharacteristics command)
        {
            var notification = await db.NotificationApplications.Include(n => n.ShipmentInfo).SingleAsync(n => n.Id == command.NotificationId);
            var physicalCharacteristics = mapper.Map(command.PhysicalCharacteristics);

            foreach (var physicalCharacteristic in physicalCharacteristics)
            {
                if (physicalCharacteristic == Domain.Notification.PhysicalCharacteristicType.Other)
                {
                    notification.AddPhysicalCharacteristic(physicalCharacteristic, command.OtherDescription);
                }
                else
                {
                    notification.AddPhysicalCharacteristic(physicalCharacteristic);
                }
            }

            await db.SaveChangesAsync();

            return notification.WasteType.Id;
        }
    }
}