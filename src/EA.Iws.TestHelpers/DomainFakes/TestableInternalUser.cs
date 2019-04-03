namespace EA.Iws.TestHelpers.DomainFakes
{
    using Core.Notification;
    using Domain;
    using Helpers;

    public class TestableInternalUser : InternalUser
    {
        public new string UserId
        {
            get { return base.UserId; }
            set { ObjectInstantiator<InternalUser>.SetProperty(x => x.UserId, value, this); }
        }

        public new UKCompetentAuthority CompetentAuthority
        {
            get { return base.CompetentAuthority; }
            set { ObjectInstantiator<InternalUser>.SetProperty(x => x.CompetentAuthority, value, this); }
        }
    }
}
