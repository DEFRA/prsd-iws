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

        public new Guid UserId
        {
            get { return base.UserId; }
            set { ObjectInstantiator<NotificationApplication>.SetProperty(x => x.UserId, value, this); }
        }

        public new NotificationType NotificationType
        {
            get { return base.NotificationType; }
            set { ObjectInstantiator<NotificationApplication>.SetProperty(x => x.NotificationType, value, this); }
        }

        public new string NotificationNumber
        {
            get { return base.NotificationNumber; }
            set { ObjectInstantiator<NotificationApplication>.SetProperty(x => x.NotificationNumber, value, this); }
        }

        public new ShipmentInfo ShipmentInfo
        {
            get { return base.ShipmentInfo; }
            set { ObjectInstantiator<NotificationApplication>.SetProperty(x => x.ShipmentInfo, value, this); }
        }

        public TestableNotificationApplication()
        {
        }
    }
}
