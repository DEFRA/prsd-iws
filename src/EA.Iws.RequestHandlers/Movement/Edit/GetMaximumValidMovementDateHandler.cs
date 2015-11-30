namespace EA.Iws.RequestHandlers.Movement.Edit
{
    using System;
    using System.Threading.Tasks;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.Movement.Edit;

    internal class GetMaximumValidMovementDateHandler : IRequestHandler<GetMaximumValidMovementDate, DateTime>
    {
        private readonly ValidMovementDateCalculator validMovementDate;

        public GetMaximumValidMovementDateHandler(ValidMovementDateCalculator validMovementDate)
        {
            this.validMovementDate = validMovementDate;
        }

        public async Task<DateTime> HandleAsync(GetMaximumValidMovementDate message)
        {
            return await validMovementDate.Maximum(message.MovementId);
        }
    }
}