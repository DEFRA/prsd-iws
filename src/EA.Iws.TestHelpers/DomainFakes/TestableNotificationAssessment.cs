namespace EA.Iws.TestHelpers.DomainFakes
{
    using System;
    using Core.NotificationAssessment;
    using Domain.NotificationAssessment;
    using Helpers;

    public class TestableNotificationAssessment : NotificationAssessment
    {
        public new Guid Id
        {
            get { return base.Id; }
            set { ObjectInstantiator<NotificationAssessment>.SetProperty(na => na.Id, value, this); }
        }

        public new Guid NotificationApplicationId
        {
            get { return base.NotificationApplicationId; }
            set { ObjectInstantiator<NotificationAssessment>.SetProperty(na => na.NotificationApplicationId, value, this); }
        }

        public new NotificationStatus Status
        {
            get { return base.Status; }
            set { ObjectInstantiator<NotificationAssessment>.SetProperty(na => na.Status, value, this); }
        }
    }
}
