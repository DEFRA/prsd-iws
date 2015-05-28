namespace EA.Iws.Domain.Notification
{
    using System.Collections.Generic;
    using Prsd.Core.Domain;

    public class PackagingInfo : Entity
    {
        public List<PackagingType> PackagingTypes { get; set; }

        public string OtherDescription { get; set; }
    }
}
