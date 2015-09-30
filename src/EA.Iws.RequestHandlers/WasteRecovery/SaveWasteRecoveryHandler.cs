namespace EA.Iws.RequestHandlers.WasteRecovery
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication.WasteRecovery;
    using Prsd.Core.Mediator;
    using Requests.WasteRecovery;

    public class SaveWasteRecoveryHandler : IRequestHandler<SaveWasteRecovery, bool>
    {
        private readonly IWasteRecoveryRepository repository;
        private readonly IwsContext context;

        public SaveWasteRecoveryHandler(IWasteRecoveryRepository repository, IwsContext context)
        {
            this.context = context;
            this.repository = repository;
        }

        public async Task<bool> HandleAsync(SaveWasteRecovery message)
        {
            var wasteRecovery = await repository.GetByNotificationId(message.NotificationId);

            var newPercentage = new Percentage(message.PercentageRecoverable);
            var newEstimatedValue = new EstimatedValue(message.EstimatedValue.Unit, message.EstimatedValue.Amount);
            var newRecoveryCost = new RecoveryCost(message.RecoveryCost.Unit, message.RecoveryCost.Amount);

            if (wasteRecovery == null)
            {
                wasteRecovery = new WasteRecovery(
                    message.NotificationId,
                    newPercentage,
                    newEstimatedValue,
                    newRecoveryCost);

                context.WasteRecoveries.Add(wasteRecovery);
            }
            else
            {
                wasteRecovery.Update(newPercentage, newEstimatedValue, newRecoveryCost);
            }

            await context.SaveChangesAsync();

            return true;
        }
    }
}
