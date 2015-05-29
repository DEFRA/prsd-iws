namespace EA.Iws.Domain.Notification
{
    using Prsd.Core.Domain;

    public class PackagingInfo : Entity
    {
        public PackagingType PackagingType { get; set; }

        public string OtherDescription { get; set; }

        public PackagingInfo()
        {
        }

        public PackagingInfo(PackagingType packagingType)
        {
            PackagingType = packagingType;
        }
    }
}
