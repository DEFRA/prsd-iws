namespace EA.Iws.Web.Areas.Admin.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Core.Admin.Search;

    public class BasicSearchViewModel
    {
        [Required]
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }

        public SelectList SearchOnSelectList { get; set; }

        public List<BasicSearchResult> SearchResults { get; set; }

        public bool HasSearched { get; set; }
    }
}