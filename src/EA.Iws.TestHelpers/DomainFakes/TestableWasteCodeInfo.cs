namespace EA.Iws.TestHelpers.DomainFakes
{
    using Core.WasteCodes;
    using Domain.NotificationApplication;

    public class TestableWasteCodeInfo : WasteCodeInfo
    {
        public new WasteCode WasteCode
        {
            get { return base.WasteCode; }
            set { base.WasteCode = value; }
        }

        public new string CustomCode
        {
            get { return base.CustomCode; }
            set { base.CustomCode = value; }
        }

        public new bool IsNotApplicable
        {
            get { return base.IsNotApplicable; }
            set { base.IsNotApplicable = value; }
        }

        public new CodeType CodeType
        {
            get { return base.CodeType; }
            set { base.CodeType = value; }
        }

        public TestableWasteCodeInfo()
        {
        }

        public TestableWasteCodeInfo(WasteCode wasteCode, string customCode = null)
        {
            this.CodeType = wasteCode.CodeType;
            this.WasteCode = wasteCode;
            this.CustomCode = customCode;
        }
    }
}
