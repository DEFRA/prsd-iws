namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using System.Collections.Generic;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using Prsd.Core.Extensions;

    public class TransportRoute : Entity
    {
        protected TransportRoute()
        {
        }

        public TransportRoute(Guid importNotificationId, StateOfExport stateOfExport, StateOfImport stateOfImport)
        {
            Guard.ArgumentNotDefaultValue(() => importNotificationId, importNotificationId);
            Guard.ArgumentNotNull(() => stateOfExport, stateOfExport);
            Guard.ArgumentNotNull(() => stateOfImport, stateOfImport);

            ImportNotificationId = importNotificationId;
            StateOfExport = stateOfExport;
            StateOfImport = stateOfImport;
        }

        public Guid ImportNotificationId { get; private set; }

        public virtual StateOfExport StateOfExport { get; private set; }

        public virtual StateOfImport StateOfImport { get; private set; }

        protected virtual ICollection<TransitState> TransitStateCollection { get; set; }

        public IEnumerable<TransitState> TransitStates
        {
            get { return TransitStateCollection.ToSafeIEnumerable(); }
        }

        public void SetTransitStates(TransitStateList transitStates)
        {
            TransitStateCollection = new List<TransitState>(transitStates);
        }
    }
}