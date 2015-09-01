namespace EA.Iws.TestHelpers.DomainFakes
{
    using Domain.NotificationApplication;
    using Helpers;

    public class TestableOperationInfo : OperationInfo
    {
        public new OperationCode OperationCode
        {
            get { return base.OperationCode; }
            set { ObjectInstantiator<OperationInfo>.SetProperty(x => x.OperationCode, value, this); }
        }
    }
}
