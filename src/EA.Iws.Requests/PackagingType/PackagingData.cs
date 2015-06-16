namespace EA.Iws.Requests.PackagingType
{
    using System.Collections.Generic;

    public class PackagingData
    {
        public PackagingData()
        {
            PackagingTypes = new List<PackagingType>();
        }

        public IList<PackagingType> PackagingTypes { get; private set; }

        public string OtherDescription { get; set; }
    }
}