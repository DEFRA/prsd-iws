namespace EA.Iws.Domain.NotificationApplication
{
    using System.Collections.Generic;

    public partial class NotificationApplication
    {
        public void SetPackagingInfo(IEnumerable<PackagingInfo> packagingInfos)
        {
            PackagingInfosCollection.Clear();

            foreach (var packagingInfo in packagingInfos)
            {
                PackagingInfosCollection.Add(packagingInfo);
            }
        }
    }
}