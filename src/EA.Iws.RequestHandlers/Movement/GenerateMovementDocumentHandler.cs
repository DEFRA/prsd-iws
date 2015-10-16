namespace EA.Iws.RequestHandlers.Movement
{
    using System.Threading.Tasks;
    using Domain;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class GenerateMovementDocumentHandler : IRequestHandler<GenerateMovementDocument, byte[]>
    {
        private readonly IMovementDocumentGenerator documentGenerator;

        public GenerateMovementDocumentHandler(IMovementDocumentGenerator documentGenerator)
        {
            this.documentGenerator = documentGenerator;
        }

        public async Task<byte[]> HandleAsync(GenerateMovementDocument message)
        {
            return await documentGenerator.Generate(message.Id);
        }
    }
}