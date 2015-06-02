namespace EA.Iws.Web.ViewModels.WasteOperations
{
    using System;
    using System.Collections.Generic;
    using Shared;

    public class OperationCodesViewModel
    {
        public CheckBoxCollectionViewModel Codes { get; set; }

        public Dictionary<string, string> CodeInformation { get; set; }

        public Guid NotificationId { get; set; }
    }
}