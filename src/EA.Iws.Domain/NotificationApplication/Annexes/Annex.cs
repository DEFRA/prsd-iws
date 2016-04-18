namespace EA.Iws.Domain.NotificationApplication.Annexes
{
    using System;
    using Prsd.Core;

    public class Annex
    {
        public Guid? FileId { get; private set; }

        protected Annex()
        {
        }

        public Annex(Guid fileId)
        {
            Guard.ArgumentNotDefaultValue(() => fileId, fileId);

            FileId = fileId;
        }
    }
}