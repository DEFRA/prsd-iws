namespace EA.Iws.Web.Areas.Admin.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Core.Admin.Search;
    using Requests.Admin;

    public class BasicSearchViewModel
    {
        [Required]
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }

        public SelectList SearchOnSelectList { get; set; }

        public List<BasicSearchResult> SearchResults { get; set; }

        public GetBasicSearchResults ToRequest()
        {
            return new GetBasicSearchResults(SearchTerm);
        }
    }
}