namespace EA.Iws.Domain.ImportNotification
{
    using System.Collections;
    using System.Collections.Generic;

    public class FacilityList : IEnumerable<Facility>
    {
        private readonly List<Facility> facilities; 

        public FacilityList(IEnumerable<Facility> facilities)
        {
            this.facilities = new List<Facility>(facilities);
        }

        public IEnumerator<Facility> GetEnumerator()
        {
            return facilities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return facilities.GetEnumerator();
        }
    }
}