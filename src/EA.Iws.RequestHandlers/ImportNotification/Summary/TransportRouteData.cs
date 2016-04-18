namespace EA.Iws.RequestHandlers.ImportNotification.Summary
{
    using System.Collections.Generic;
    using Core.ImportNotification.Summary;

    internal class TransportRouteData
    {
        public bool HasNoTransitStates { get; private set; }

        public IList<TransitState> TransitStates { get; private set; }

        public StateOfExport StateOfExport { get; private set; }

        public StateOfImport StateOfImport { get; private set; }

        public TransportRouteData(IList<TransitState> transitStates, 
            StateOfExport stateOfExport, 
            StateOfImport stateOfImport,
            bool hasNoTransitStates)
        {
            TransitStates = transitStates;
            StateOfExport = stateOfExport;
            StateOfImport = stateOfImport;
            HasNoTransitStates = hasNoTransitStates;
        }
    }
}
