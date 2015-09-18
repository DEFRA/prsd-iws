namespace EA.Iws.Requests.Movement
{
    using System;
    using Prsd.Core.Mediator;

    public class GenerateMovementDocument : IRequest<byte[]>
    {
        public Guid Id { get; private set; }

        public GenerateMovementDocument(Guid id)
        {
            Id = id;
        }
    }
}
