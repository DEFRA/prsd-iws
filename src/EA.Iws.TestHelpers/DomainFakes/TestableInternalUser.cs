namespace EA.Iws.TestHelpers.DomainFakes
{
    using Domain;
    using Helpers;

    public class TestableInternalUser : InternalUser
    {
        public new string UserId
        {
            get { return base.UserId; }
            set { ObjectInstantiator<InternalUser>.SetProperty(x => x.UserId, value, this); }
        }
    }
}
