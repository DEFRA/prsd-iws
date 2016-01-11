namespace EA.Iws.Domain.NotificationApplication.Annexes
{
    using System;
    using Prsd.Core.Domain;

    public class DeleteAnnexEvent : IEvent
    {
        public Guid FileId { get; private set; }

        public DeleteAnnexEvent(Guid fileId)
        {
            FileId = fileId;
        }
    }
}