namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.ChemicalComposition
{
    using System;
    using Web.ViewModels.Shared;

    public class ChemicalCompositionTypeViewModel
    {
        public Guid NotificationId { get; set; }

        public RadioButtonStringCollectionViewModel ChemicalCompositionType { get; set; }
    }
}