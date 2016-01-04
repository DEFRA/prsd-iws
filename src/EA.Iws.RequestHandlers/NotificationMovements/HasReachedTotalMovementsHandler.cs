namespace EA.Iws.RequestHandlers.NotificationMovements
{
    using System.Threading.Tasks;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements;

    internal class HasReachedTotalMovementsHandler : IRequestHandler<HasReachedTotalMovements, bool>
    {
        private readonly NumberOfMovements numberOfMovements;

        public HasReachedTotalMovementsHandler(NumberOfMovements numberOfMovements)
        {
            this.numberOfMovements = numberOfMovements;
        }

        public async Task<bool> HandleAsync(HasReachedTotalMovements message)
        {
            return await numberOfMovements.HasMaximum(message.NotificationId);
        }
    }
}