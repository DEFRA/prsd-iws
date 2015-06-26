namespace EA.Iws.RequestHandlers.WasteType
{
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.WasteType;
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using CodeType = Domain.Notification.CodeType;

    internal class SetOptionalWasteCodesHandler : IRequestHandler<SetOptionalWasteCodes, Guid>
    {
        private readonly IwsContext db;
        private readonly IMap<Core.WasteType.CodeType, CodeType> mapper;
        public SetOptionalWasteCodesHandler(IwsContext db, IMap<Core.WasteType.CodeType, CodeType> mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }
        public async Task<Guid> HandleAsync(SetOptionalWasteCodes command)
        {
            var notification = await db.NotificationApplications.SingleAsync(n => n.Id == command.NotificationId);
            foreach (var optionalWasteCode in command.OptionalWasteCodes)
            {
                var code = mapper.Map(optionalWasteCode.CodeType);
                var wasteCode = await db.WasteCodes.SingleAsync(w => w.CodeType.Value == code.Value);

                notification.AddWasteCode(WasteCodeInfo.CreateOptionalWasteCodeInfo(wasteCode, optionalWasteCode.OptionalCode, optionalWasteCode.OptionalDescription));
            }
            await db.SaveChangesAsync();
            return notification.WasteType.Id;
        }
    }
}