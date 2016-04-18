namespace EA.Iws.Web.Areas.Admin.ViewModels.Home
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Core.Admin.Search;
    using Core.NotificationAssessment;
    using Requests.Admin.Search;

    public class BasicSearchViewModel : IValidatableObject
    {
        [Required]
        [Display(Name = "SearchTerm", ResourceType = typeof(BasicSearchViewModelResources))]
        public string SearchTerm { get; set; }

        public SelectList SearchOnSelectList { get; set; }

        public IList<BasicSearchResult> ExportSearchResults { get; set; }

        public IList<ImportSearchResult> ImportSearchResults { get; set; }

        public IEnumerable<NotificationAttentionSummaryTableData> AttentionSummaryTable { get; set; }

        public bool HasSearched { get; set; }

        public BasicSearchViewModel()
        {
            ImportSearchResults = new List<ImportSearchResult>();
        }

        public SearchExportNotifications ToRequest()
        {
            return new SearchExportNotifications(SearchTerm);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                yield return new ValidationResult(BasicSearchViewModelResources.SearchTermRequired, new[] { "SearchTerm" });
            }
        }
    }
}