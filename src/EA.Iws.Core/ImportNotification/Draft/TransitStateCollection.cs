namespace EA.Iws.Core.ImportNotification.Draft
{
    using System.Collections.Generic;

    public class TransitStateCollection
    {
        public List<TransitState> TransitStates { get; set; }

        public bool HasNoTransitStates { get; set; }

        public TransitStateCollection()
        {
            TransitStates = new List<TransitState>();
        }
    }
}
