namespace EA.Iws.Web.Areas.Reports.ViewModels.RdfSrfWood
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.WasteType;
    using Infrastructure.Validation;
    using Prsd.Core;
    using Prsd.Core.Helpers;
    using Web.ViewModels.Shared;

    public class IndexViewModel : IValidatableObject
    {
        public IndexViewModel()
        {
            FromDate = new OptionalDateInputViewModel(SystemTime.Now.AddMonths(-1), true);
            ToDate = new OptionalDateInputViewModel(SystemTime.Now, true);

            ChemicalCompositions = new SelectList(
                EnumHelper.GetValues(typeof(ChemicalComposition))
                    .Where(p => p.Key != (int)ChemicalComposition.Other),
                "Key",
                "Value",
                null);
        }

        [Display(Name = "FromDate", ResourceType = typeof(IndexViewModelResources))]
        [RequiredDateInput(ErrorMessageResourceName = "FromDateRequired", ErrorMessageResourceType = typeof(IndexViewModelResources))]
        public OptionalDateInputViewModel FromDate { get; set; }

        [Display(Name = "ToDate", ResourceType = typeof(IndexViewModelResources))]
        [RequiredDateInput(ErrorMessageResourceName = "ToDateRequired", ErrorMessageResourceType = typeof(IndexViewModelResources))]
        public OptionalDateInputViewModel ToDate { get; set; }

        [Display(Name = "WasteType", ResourceType = typeof(IndexViewModelResources))]
        [Required(ErrorMessageResourceName = "WasteTypeRequired",
            ErrorMessageResourceType = typeof(IndexViewModelResources))]
        public ChemicalComposition ChemicalComposition { get; set; }

        public SelectList ChemicalCompositions { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FromDate.AsDateTime() > ToDate.AsDateTime())
            {
                yield return new ValidationResult(IndexViewModelResources.FromDateBeforeToDate, new[] { "FromDate" });
            }
        }
    }
}