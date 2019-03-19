namespace EA.Iws.Web.Areas.Reports.ViewModels.ExportMovements
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using EA.Iws.Core.Reports;
    using EA.Prsd.Core;
    using EA.Prsd.Core.Helpers;
    using Infrastructure.Validation;
    using Web.Areas.Reports.Views.ExportMovements;
    using Web.ViewModels.Shared;

    public class IndexViewModel : IValidatableObject
    {
        [Display(Name = "From", ResourceType = typeof(IndexViewModelResources))]
        public OptionalDateInputViewModel From { get; set; }

        [Display(Name = "To", ResourceType = typeof(IndexViewModelResources))]
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
            organisationOptions.Add(-1, "View all");
            var orderedOrganisationOptions = organisationOptions.OrderBy(p => p.Key);
            OrganisationTypesSelectList = new SelectList(orderedOrganisationOptions, "Key", "Value", null);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if ((SelectedOrganistationFilter != null && SelectedOrganistationFilter != "-1") && (OrganisationName == null || OrganisationName == string.Empty))
            {
                yield return new ValidationResult(IndexResources.OrganisationNameRequiredError, new[] { "OrganisationName" });
            }

            DateTime startDate;
            bool isValidstartDate = SystemTime.TryParse(this.From.Year.GetValueOrDefault(), this.From.Month.GetValueOrDefault(), this.From.Day.GetValueOrDefault(), out startDate);
            if (!isValidstartDate)
            {
                yield return new ValidationResult("Please enter a valid from date", new[] { "From" });
            }

            DateTime endDate;
            bool isValidEndDate = SystemTime.TryParse(this.To.Year.GetValueOrDefault(), this.To.Month.GetValueOrDefault(), this.To.Day.GetValueOrDefault(), out endDate);
            if (!isValidEndDate)
            {
                yield return new ValidationResult("Please enter a valid to date", new[] { "To" });
            }

            if (!isValidEndDate || !isValidstartDate)
            {
                // Stop further validation if either date is not a valid date
                yield break;
            }

            if (From.AsDateTime() > To.AsDateTime())
            {
                yield return new ValidationResult(IndexViewModelResources.FromDateBeforeToDate, new[] { "From" });
            }
        }
    }
}