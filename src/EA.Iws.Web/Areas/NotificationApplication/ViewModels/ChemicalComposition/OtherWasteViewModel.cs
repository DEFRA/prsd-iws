﻿namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.ChemicalComposition
{
    using EA.Iws.Core.WasteType;
    using System;
    using System.ComponentModel.DataAnnotations;
    using Views.ChemicalComposition;

    public class OtherWasteViewModel
    {
        public Guid NotificationId { get; set; }

        [Required]
        [StringLength(70, ErrorMessageResourceName = "DescriptionLength", ErrorMessageResourceType = typeof(ChemicalCompositionResources))]
        public string Description { get; set; }

        public WasteCategoryType WasteCategoryType { get; set; }
    }
}