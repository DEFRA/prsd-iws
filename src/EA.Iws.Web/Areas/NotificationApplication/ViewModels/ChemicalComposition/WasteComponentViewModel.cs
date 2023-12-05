namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.ChemicalComposition
{
    using System;
    using Web.ViewModels.Shared;

    public class WasteComponentViewModel
    {
        public Guid NotificationId { get; set; }

        public CheckBoxCollectionViewModel WasteComponentTypes { get; set; }
    }
}