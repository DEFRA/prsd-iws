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
    using PhysicalCharacteristicType = Requests.WasteType.PhysicalCharacteristicType;

    internal class SetEwcCodesHandler : IRequestHandler<SetEwcCodes, Guid>
    {
        private readonly IwsContext db;
        private readonly IMap<IList<PhysicalCharacteristicType>, IList<Domain.Notification.PhysicalCharacteristicType>> mapper;

        public SetEwcCodesHandler(IwsContext db, IMap<IList<PhysicalCharacteristicType>, IList<Domain.Notification.PhysicalCharacteristicType>> mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<Guid> HandleAsync(SetEwcCodes command)
        {
            var notification = await db.NotificationApplications.Include(n => n.ShipmentInfo).SingleAsync(n => n.Id == command.NotificationId);

            foreach (var ewcWasteCode in command.EwcWasteCodes)
            {
                var id = ewcWasteCode.Id;
                var wasteCode = await db.WasteCodes.SingleAsync(w => w.Id == id);
                notification.AddWasteCode(wasteCode);
            }

            await db.SaveChangesAsync();

            return notification.WasteType.Id;
        }
    }
}