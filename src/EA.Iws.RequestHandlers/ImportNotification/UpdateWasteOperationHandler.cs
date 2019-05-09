namespace EA.Iws.RequestHandlers.ImportNotification
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;

    internal class UpdateWasteOperationHandler : IRequestHandler<UpdateWasteOperation, Guid>
    {
        private readonly ImportNotificationContext context;
        private readonly IWasteOperationRepository wasteOperationRepository;

        public UpdateWasteOperationHandler(ImportNotificationContext context,
            IWasteOperationRepository wasteOperationRepository)
        {
            this.context = context;
            this.wasteOperationRepository = wasteOperationRepository;
        }

        public async Task<Guid> HandleAsync(UpdateWasteOperation message)
        {
            var codes = message.OperationCodes.Select(x => new WasteOperationCode(x)).ToList();

            var wasteOperation = await wasteOperationRepository.GetByNotificationId(message.ImportNotificationId);

            wasteOperation.SetOperationCodes(codes);
            wasteOperation.SetTechnologyEmployed(message.TechnologyEmployed);

            await context.SaveChangesAsync();

            return wasteOperation.Id;
        }
    }
}
