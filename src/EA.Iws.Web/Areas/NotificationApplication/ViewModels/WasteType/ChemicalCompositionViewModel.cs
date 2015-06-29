namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteType
{
    using System;
    using Web.ViewModels.Shared;

    public class ChemicalCompositionViewModel
    {
        public Guid NotificationId { get; set; }

        public RadioButtonStringCollectionViewModel ChemicalCompositionType { get; set; }
    }
}