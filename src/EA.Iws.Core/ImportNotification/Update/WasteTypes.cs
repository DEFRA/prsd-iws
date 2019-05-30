namespace EA.Iws.Core.ImportNotification.Update
{
    using System;
    using System.Collections.Generic;
    using WasteCodes;
    using WasteType;

    public class WasteTypes
    {
        public string Name { get; set; }

        public bool BaselCodeNotListed { get; set; }

        public bool YCodeNotApplicable { get; set; }

        public bool HCodeNotApplicable { get; set; }

        public bool UnClassNotApplicable { get; set; }

        public Guid? SelectedBaselCode { get; set; }

        public List<Guid> SelectedEwcCodes { get; set; }

        public List<Guid> SelectedYCodes { get; set; }

        public List<Guid> SelectedHCodes { get; set; }

        public List<Guid> SelectedUnClasses { get; set; }

        public Guid ImportNotificationId { get; private set; }

        public IList<WasteCodeData> AllCodes { get; set; }

        public WasteTypes(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;

            SelectedEwcCodes = new List<Guid>();
            SelectedHCodes = new List<Guid>();
            SelectedYCodes = new List<Guid>();
            SelectedUnClasses = new List<Guid>();
            AllCodes = new List<WasteCodeData>();
        }

        public WasteTypes()
        {
            SelectedEwcCodes = new List<Guid>();
            SelectedHCodes = new List<Guid>();
            SelectedYCodes = new List<Guid>();
            SelectedUnClasses = new List<Guid>();
            AllCodes = new List<WasteCodeData>();
        }
    }
}
