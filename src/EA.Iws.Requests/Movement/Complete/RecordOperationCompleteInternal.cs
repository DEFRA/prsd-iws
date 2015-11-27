namespace EA.Iws.Requests.Movement.Complete
{
    using System;
    using Prsd.Core.Mediator;

    public class RecordOperationCompleteInternal : IRequest<bool>
    {
        public Guid Id { get; private set; }

        public DateTime CompleteDate { get; private set; }

        public RecordOperationCompleteInternal(Guid id, DateTime completeDate)
        {
            Id = id;
            CompleteDate = completeDate;
        }
    }
}