namespace EA.Iws.RequestHandlers.NotificationMovements.Create
{
    using System.Threading.Tasks;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.Create;

    internal class HasExceededConsentedQuantityHandler : IRequestHandler<HasExceededConsentedQuantity, bool>
    {
        private readonly NotificationMovementsQuantity movementsQuantity;

        public HasExceededConsentedQuantityHandler(NotificationMovementsQuantity movementsQuantity)
        {
            this.movementsQuantity = movementsQuantity;
        }

        public async Task<bool> HandleAsync(HasExceededConsentedQuantity message)
        {
            var remaining = await movementsQuantity.Remaining(message.NotificationId);

            return message.Quantity > remaining;
        }
    }
}