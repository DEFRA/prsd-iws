namespace EA.Iws.RequestHandlers.NotificationMovements.Create
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.Create;

    internal class GenerateMovementNumbersHandler : IRequestHandler<GenerateMovementNumbers, IList<int>>
    {
        private readonly MovementNumberGenerator generator;

        public GenerateMovementNumbersHandler(MovementNumberGenerator generator)
        {
            this.generator = generator;
        }

        public async Task<IList<int>> HandleAsync(GenerateMovementNumbers message)
        {
            return await generator.Generate(message.NotificationId, message.NewMovementsCount);
        }
    }
}