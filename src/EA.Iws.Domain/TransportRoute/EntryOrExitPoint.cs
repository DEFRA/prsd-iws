namespace EA.Iws.Domain.TransportRoute
{
    using System;

    public class EntryOrExitPoint
    {
        public Guid Id { get; protected set; }

        public string Name { get; protected set; }

        public virtual Country Country { get; protected set; }

        protected EntryOrExitPoint()
        {
        }
    }
}
