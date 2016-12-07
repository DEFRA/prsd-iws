namespace EA.Iws.Core.ImportNotification.Draft
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    [DisplayName("Waste codes")]
    public class WasteType : IDraftEntity
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

        public WasteType(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }

        internal WasteType()
        {
        }
    }
}