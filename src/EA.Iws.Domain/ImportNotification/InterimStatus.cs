namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using Prsd.Core.Domain;

    public class InterimStatus : Entity
    {
        public bool IsInterim { get; private set; }

        public Guid ImportNotificationId { get; private set; }

        protected InterimStatus()
        {
        }

        public InterimStatus(Guid importNotificationId, bool isInterim)
        {
            IsInterim = isInterim;
            ImportNotificationId = importNotificationId;
        }
    }
}
