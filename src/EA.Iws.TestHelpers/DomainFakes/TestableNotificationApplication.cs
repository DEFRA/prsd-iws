namespace EA.Iws.TestHelpers.DomainFakes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.NotificationApplication;
    using Helpers;

    public class TestableNotificationApplication : NotificationApplication
    {
        public new Guid Id
        {
            get { return base.Id; }
            set { ObjectInstantiator<NotificationApplication>.SetProperty(x => x.Id, value, this); }
        }

        public new IList<WasteCodeInfo> WasteCodes
        {
            get { return base.WasteCodes.ToArray(); }
            set { WasteCodeInfoCollection = value; }
        }

        public TestableNotificationApplication()
        {
        }
    }
}
