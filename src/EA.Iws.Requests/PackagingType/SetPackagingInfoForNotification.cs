namespace EA.Iws.Requests.PackagingType
{
    using System;
    using System.Collections.Generic;
    using Prsd.Core.Mediator;

    public class SetPackagingInfoForNotification : IRequest<Guid>
    {
        public SetPackagingInfoForNotification(List<PackagingType> packagingTypes, Guid notificationId, string otherDescription)
        {
            PackagingTypes = packagingTypes;

            NotificationId = notificationId;

            OtherDescription = otherDescription;
        }

        public List<PackagingType> PackagingTypes { get; private set; }

        public Guid NotificationId { get; private set; }

        public string OtherDescription { get; private set; }
    }
}