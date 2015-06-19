namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteOperations
{
    using System;
    using System.Collections.Generic;
    using Web.ViewModels.Shared;

    public class OperationCodesViewModel
    {
        public CheckBoxCollectionViewModel Codes { get; set; }

        public Dictionary<string, string> CodeInformation { get; set; }

        public Guid NotificationId { get; set; }
    }
}