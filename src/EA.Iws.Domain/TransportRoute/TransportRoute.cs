namespace EA.Iws.Domain.TransportRoute
{
    using System.Collections.Generic;

    public class TransportRoute
    {
        public StateOfExport StateOfExport { get; protected set; }

        public StateOfImport StateOfImport { get; protected set; }

        public List<TransitState> TransitStates { get; protected set; }

        protected TransportRoute()
        {
        }
    }
}
