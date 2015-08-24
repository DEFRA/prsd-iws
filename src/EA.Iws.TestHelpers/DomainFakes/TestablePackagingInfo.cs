namespace EA.Iws.TestHelpers.DomainFakes
{
    using System;
    using Domain.NotificationApplication;
    using Helpers;

    public class TestablePackagingInfo : PackagingInfo
    {
        public new Guid Id
        {
            get { return base.Id; }
            set { ObjectInstantiator<PackagingInfo>.SetProperty(x => x.Id, value, this); }
        }

        public new PackagingType PackagingType
        {
            get { return base.PackagingType; }
            set { ObjectInstantiator<PackagingInfo>.SetProperty(x => x.PackagingType, value, this); }
        }

        public new string OtherDescription
        {
            get { return base.OtherDescription; }
            set { ObjectInstantiator<PackagingInfo>.SetProperty(x => x.OtherDescription, value, this); }
        }
    }
}
