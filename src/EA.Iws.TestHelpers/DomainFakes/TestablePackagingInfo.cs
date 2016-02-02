namespace EA.Iws.TestHelpers.DomainFakes
{
    using System;
    using Core.PackagingType;
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

        public static readonly TestablePackagingInfo Box = new TestablePackagingInfo
        {
            Id = new Guid("9CAFC0EA-6E12-4E6E-B59F-2E2BE0A63425"),
            PackagingType = PackagingType.Box
        };

        public static readonly TestablePackagingInfo LargeSacks = new TestablePackagingInfo
        {
            Id = new Guid("FCBDF4ED-156E-44D2-8485-DF29C432280B"),
            PackagingType = PackagingType.Other,
            OtherDescription = "Large sacks"
        };
    }
}
