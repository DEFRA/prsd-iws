namespace EA.Iws.Web.Areas.Reports.ViewModels.ExportMovements
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Infrastructure.Validation;
    using Web.Areas.Reports.Views.ExportMovements;
    using Web.ViewModels.Shared;

    public class IndexViewModel : IValidatableObject
    {
        [Display(Name = "From", ResourceType = typeof(IndexViewModelResources))]
        [RequiredDateInput(ErrorMessageResourceName = "FromRequired", ErrorMessageResourceType = typeof(IndexViewModelResources))]
        public OptionalDateInputViewModel From { get; set; }

        [Display(Name = "To", ResourceType = typeof(IndexViewModelResources))]
        [RequiredDateInput(ErrorMessageResourceName = "ToRequired", ErrorMessageResourceType = typeof(IndexViewModelResources))]
        public OptionalDateInputViewModel To { get; set; }

        public SelectList OrganisationTypes
        {
            get
            {
                var organisations = new List<SelectListItem>()
                {
                    new SelectListItem { Text = string.Empty, Value = string.Empty},
                    new SelectListItem { Text = "Notifier name", Value = "notifier"},
                    new SelectListItem { Text = "Consignee  name", Value = "consignee "}
                };

                return new SelectList(organisations, "Value", "Text", string.Empty);
            }
        }

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
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (From.AsDateTime() > To.AsDateTime())
            {
                yield return new ValidationResult(IndexViewModelResources.FromDateBeforeToDate, new[] { "FromDate" });
            }

            if ((SelectedOrganistationFilter != null && SelectedOrganistationFilter != string.Empty) && (OrganisationName == null || OrganisationName == string.Empty))
            {
                yield return new ValidationResult(IndexResources.OrganisationNameRequiredError, new[] { "OrganisationName" });
            }
        }
    }
}