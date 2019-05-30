namespace EA.Iws.Requests.NotificationAssessment
{
    using System.Collections.Generic;
    using Core.TransitState;
    using Core.TransportRoute;

    public class TransitStateWithEntryOrExitPointsData
    {
        public TransitStateData TransitState { get; set; }

        public IEnumerable<EntryOrExitPointData> EntryOrExitPoints { get; set; }
    }
}
