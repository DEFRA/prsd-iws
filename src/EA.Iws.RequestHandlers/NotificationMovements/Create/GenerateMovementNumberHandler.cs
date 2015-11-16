namespace EA.Iws.RequestHandlers.NotificationMovements.Create
{
    using System.Threading.Tasks;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.Create;

    internal class GenerateMovementNumberHandler : IRequestHandler<GenerateMovementNumber, int>
    {
        private readonly MovementNumberGenerator generator;

        public GenerateMovementNumberHandler(MovementNumberGenerator generator)
        {
            this.generator = generator;
        }

        public async Task<int> HandleAsync(GenerateMovementNumber message)
        {
            return await generator.Generate(message.NotificationId);
        }
    }
}