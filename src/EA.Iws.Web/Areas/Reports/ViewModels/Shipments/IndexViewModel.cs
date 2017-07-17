namespace EA.Iws.Web.Areas.Reports.ViewModels.Shipments
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Core.Reports;
    using Infrastructure.Validation;
    using Prsd.Core.Helpers;
    using Web.ViewModels.Shared;

    public class IndexViewModel : IValidatableObject
    {
        public IndexViewModel()
        {
            From = new OptionalDateInputViewModel(true);
            To = new OptionalDateInputViewModel(true);

            DateSelectList = new SelectList(EnumHelper.GetValues(typeof(ShipmentsReportDates)), "Key", "Value", null);
        }

        [Display(Name = "From", ResourceType = typeof(ExportStats.IndexViewModelResources))]
        [RequiredDateInput(ErrorMessageResourceName = "FromRequired",
            ErrorMessageResourceType = typeof(ExportStats.IndexViewModelResources))]
        public OptionalDateInputViewModel From { get; set; }

        [Display(Name = "To", ResourceType = typeof(ExportStats.IndexViewModelResources))]
        [RequiredDateInput(ErrorMessageResourceName = "ToRequired",
            ErrorMessageResourceType = typeof(ExportStats.IndexViewModelResources))]
        public OptionalDateInputViewModel To { get; set; }

        [Display(Name = "DateType", ResourceType = typeof(IndexViewModelResources))]
        [Required(ErrorMessageResourceName = "DateTypeRequired", ErrorMessageResourceType = typeof(IndexViewModelResources))]
        public ShipmentsReportDates DateType { get; set; } 

        public SelectList DateSelectList { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (From.AsDateTime() > To.AsDateTime())
            {
                yield return new ValidationResult(IndexViewModelResources.FromDateBeforeToDate, new[] { "FromDate" });
            }
        }
    }
}