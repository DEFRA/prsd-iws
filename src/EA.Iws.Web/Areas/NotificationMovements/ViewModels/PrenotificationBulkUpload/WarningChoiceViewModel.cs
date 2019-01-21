namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.PrenotificationBulkUpload
{
    using System.Collections.Generic;
    using Web.ViewModels.Shared;

    public class WarningChoiceViewModel
    {
        public WarningChoiceViewModel()
        {
            List<string> warningChoices = new List<string>()
            {
                "Return to upload the accompanying shipment movement documents",
                "Leave the process and return to notification options"
            };
            WarningChoices = new RadioButtonStringCollectionViewModel(warningChoices);
        }

        public RadioButtonStringCollectionViewModel WarningChoices { get; set; }
    }
}