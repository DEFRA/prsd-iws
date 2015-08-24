namespace EA.Iws.Web.Areas.MovementDocument.ViewModels
{
    using System;
    using Web.ViewModels.Shared;

    public class PackagingTypesViewModel
    {
        public CheckBoxCollectionViewModel PackagingTypes { get; set; }

        public Guid MovementId { get; set; }
    }
}