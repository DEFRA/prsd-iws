namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Prsd.Core.Domain;
    using Prsd.Core.Extensions;

    public class FacilityCollection : Entity
    {
        protected FacilityCollection()
        {
        }

        public FacilityCollection(Guid importNotificationId, FacilityList facilities,
            bool allFacilitiesPreconsented)
        {
            ImportNotificationId = importNotificationId;
            AllFacilitiesPreconsented = allFacilitiesPreconsented;
            FacilitiesCollection = new List<Facility>(facilities);

            AssignOrdinalPositions();
        }

        public Guid ImportNotificationId { get; private set; }

        public bool AllFacilitiesPreconsented { get; private set; }

        protected virtual ICollection<Facility> FacilitiesCollection { get; set; }

        public IEnumerable<Facility> Facilities
        {
            get { return FacilitiesCollection == null ? Enumerable.Empty<Facility>() : FacilitiesCollection.OrderBy(f => f.OrdinalPosition); }
        }

        private void AssignOrdinalPositions()
        {
            var position = 1;
            foreach (var facility in FacilitiesCollection)
            {
                facility.OrdinalPosition = position++;
            }
        }
    }
}