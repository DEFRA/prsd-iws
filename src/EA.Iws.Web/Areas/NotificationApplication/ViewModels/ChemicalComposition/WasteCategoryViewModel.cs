namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.ChemicalComposition
{
    using EA.Iws.Web.ViewModels.Shared;
    using System;

    public class WasteCategoryViewModel
    {
        public Guid NotificationId { get; set; }

        public RadioButtonStringCollectionViewModel WasteCategoryType { get; set; }
    }
}