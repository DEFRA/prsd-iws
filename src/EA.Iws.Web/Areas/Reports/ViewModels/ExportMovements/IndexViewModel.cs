namespace EA.Iws.Web.Areas.Reports.ViewModels.ExportMovements
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using EA.Iws.Core.Reports;
    using EA.Prsd.Core.Helpers;
    using Infrastructure.Validation;
    using Web.ViewModels.Shared;

    public class IndexViewModel : IValidatableObject
    {
        [Display(Name = "From", ResourceType = typeof(IndexViewModelResources))]
        [RequiredDateInput(ErrorMessageResourceName = "FromRequired", ErrorMessageResourceType = typeof(IndexViewModelResources))]
        public OptionalDateInputViewModel From { get; set; }

        [Display(Name = "To", ResourceType = typeof(IndexViewModelResources))]
        [RequiredDateInput(ErrorMessageResourceName = "ToRequired", ErrorMessageResourceType = typeof(IndexViewModelResources))]
        public OptionalDateInputViewModel To { get; set; }

        public SelectList OrganisationTypesSelectList { get; set; }

        public string SelectedOrganistationFilter { get; set; }

        private string organisationName;
        public string OrganisationName
        {
            get
            {
                if (this.SelectedOrganistationFilter == null || this.SelectedOrganistationFilter == string.Empty)
                {
                    return string.Empty;
                }

                return organisationName;
            }
            set { organisationName = value; }
        }

        public IndexViewModel()
        {
            From = new OptionalDateInputViewModel(true);
            To = new OptionalDateInputViewModel(true);

            var organisationOptions = EnumHelper.GetValues(typeof(OrganisationFilterOptions));
            organisationOptions.Add(-1, string.Empty);
            var orderedOrganisationOptions = organisationOptions.OrderBy(p => p.Key);
            OrganisationTypesSelectList = new SelectList(orderedOrganisationOptions, "Key", "Value", null);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (From.AsDateTime() > To.AsDateTime())
            {
                yield return new ValidationResult(IndexViewModelResources.FromDateBeforeToDate, new[] { "FromDate" });
            }
        }
    }
}