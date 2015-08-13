namespace EA.Iws.TestHelpers.DomainFakes
{
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

        public TestableWasteCodeInfo()
        {
        }

        public TestableWasteCodeInfo(WasteCode wasteCode, string customCode = null)
        {
            this.WasteCode = wasteCode;
            this.CustomCode = customCode;
        }
    }
}
