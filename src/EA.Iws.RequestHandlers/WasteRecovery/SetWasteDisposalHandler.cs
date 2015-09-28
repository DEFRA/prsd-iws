namespace EA.Iws.RequestHandlers.WasteRecovery
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication.WasteRecovery;
    using Prsd.Core.Mediator;
    using Requests.WasteRecovery;

    internal class SetWasteDisposalHandler : IRequestHandler<SetWasteDisposal, Guid>
    {
        private readonly IwsContext context;
        private readonly IWasteDisposalRepository repository;

        public SetWasteDisposalHandler(IWasteDisposalRepository repository, IwsContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        public async Task<Guid> HandleAsync(SetWasteDisposal message)
        {
            var wasteDisposal = await repository.GetByNotificationId(message.NotificationId);

            if (wasteDisposal == null)
            {
                wasteDisposal = new WasteDisposal(message.NotificationId, message.Method, new DisposalCost(message.Unit, message.Amount));
                context.WasteDisposals.Add(wasteDisposal);
            }
            else
            {
                wasteDisposal.UpdateWasteDisposal(message.Method, new DisposalCost(message.Unit, message.Amount));
            }

            await context.SaveChangesAsync();

            return message.NotificationId;
        }
    }
}
