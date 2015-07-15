namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteType
{
    using System;
    using Prsd.Core.Validation;

    public class OtherWasteAdditionalInformationViewModel
    {
        public Guid NotificationId { get; set; }

        [RequiredIf("HasAttachement", false, "Please enter a description or check the box")]
        public string Description { get; set; }

        public bool HasAttachement { get; set; }
    }
}