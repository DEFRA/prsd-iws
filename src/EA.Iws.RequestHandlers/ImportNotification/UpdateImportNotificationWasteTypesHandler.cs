namespace EA.Iws.RequestHandlers.ImportNotification
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;

    public class UpdateImportNotificationWasteTypesHandler : IRequestHandler<UpdateImportNotificationWasteTypes, Guid>
    {
        private readonly ImportNotificationContext context;
        private readonly IWasteTypeRepository wasteTypeRepository;
        private readonly IMapper mapper;

        public UpdateImportNotificationWasteTypesHandler(ImportNotificationContext context,
            IWasteTypeRepository wasteTypeRepository, IMapper mapper)
        {
            this.context = context;
            this.wasteTypeRepository = wasteTypeRepository;
            this.mapper = mapper;
        }

        public async Task<Guid> HandleAsync(UpdateImportNotificationWasteTypes message)
        {
            var currentWasteType = await wasteTypeRepository.GetByNotificationId(message.ImportNotificationId);
            var codes = mapper.Map<UpdateWasteCodeData>(message.WasteTypes);

            currentWasteType.Update(codes.Name, codes.BaselOecdCode, codes.EwcCode, codes.YCode,
                codes.HCode, codes.UnClass);

            await context.SaveChangesAsync();

            return currentWasteType.Id;
        }
    }
}