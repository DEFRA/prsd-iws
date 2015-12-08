namespace EA.Iws.Core.ImportNotification.Draft
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    [DisplayName("Transport route transits")]
    public class TransitStateCollection
    {
        public List<TransitState> TransitStates { get; private set; }

        public bool HasNoTransitStates { get; set; }

        public TransitStateCollection()
        {
            TransitStates = new List<TransitState>();
        }

        public void Delete(Guid id)
        {
            if (TransitStates == null || TransitStates.Count <= 0)
            {
                return;
            }

            TransitStates.RemoveAll(ts => ts.Id == id);

            ReorderTransitStates();
        }

        public void Add(TransitState transitState)
        {
            ReorderTransitStates();
            
            transitState.OrdinalPosition = TransitStates.Count > 0 ? 
                TransitStates.Select(ts => ts.OrdinalPosition).Max() + 1
                : 1;

            TransitStates.Add(transitState);
        }

        private void ReorderTransitStates()
        {
            // Where the list contains 1, 2, 3 and 2 is removed, the positions should reorder to 1, 2.
            var orderedStates = TransitStates.OrderBy(ts => ts.OrdinalPosition).ToList();

            for (int i = 0; i < orderedStates.Count; i++)
            {
                orderedStates[i].OrdinalPosition = i + 1;
            }

            TransitStates = orderedStates;
        }
    }
}
