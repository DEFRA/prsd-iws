namespace EA.Iws.Requests.Movement
{
    using System;
    using Core.Documents;
    using Prsd.Core.Mediator;

    public class GenerateMovementDocument : IRequest<FileData>
    {
        public Guid Id { get; private set; }

        public GenerateMovementDocument(Guid id)
        {
            Id = id;
        }
    }
}
