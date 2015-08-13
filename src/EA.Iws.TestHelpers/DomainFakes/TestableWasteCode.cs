namespace EA.Iws.TestHelpers.DomainFakes
{
    using System;
    using Core.WasteCodes;
    using Domain.NotificationApplication;

    public class TestableWasteCode : WasteCode
    {
        public new Guid Id 
        {
            get { return base.Id; }
            set { base.Id = value; }
        }

        public new CodeType CodeType
        {
            get { return base.CodeType; }
            set { base.CodeType = value; }
        }

        public new string Code
        {
            get { return base.Code; }
            set { base.Code = value; }
        }

        public TestableWasteCode()
        {
        }
    }
}
