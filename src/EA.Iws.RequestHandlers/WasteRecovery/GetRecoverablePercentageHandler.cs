namespace EA.Iws.RequestHandlers.WasteRecovery
{
    using System;
    using System.Threading.Tasks;
    using Domain.NotificationApplication.WasteRecovery;
    using Prsd.Core.Mediator;
    using Requests.WasteRecovery;

    internal class GetRecoverablePercentageHandler : IRequestHandler<GetRecoverablePercentage, decimal?>
    {
        private readonly IWasteRecoveryRepository repository;

        public GetRecoverablePercentageHandler(IWasteRecoveryRepository repository)
        {
            this.repository = repository;
        }

        public async Task<decimal?> HandleAsync(GetRecoverablePercentage message)
        {
            var wasteRecovery = await repository.GetByNotificationId(message.NotificationId);

            if (wasteRecovery != null)
            {
                return wasteRecovery.PercentageRecoverable.Value;
            }

            return null;
        }
    }
}
