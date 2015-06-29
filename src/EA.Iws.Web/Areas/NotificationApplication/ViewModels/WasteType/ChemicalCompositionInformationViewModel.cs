namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteType
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.WasteType;
    using Requests.WasteType;

    public class ChemicalCompositionInformationViewModel
    {
        public Guid NotificationId { get; set; }

        public List<WoodInformationData> WasteComposition { get; set; }

        public string FurtherInformation { get; set; }

        [Required]
        public string Energy { get; set; }

        public ChemicalCompositionType ChemicalCompositionType { get; set; }
    }
}