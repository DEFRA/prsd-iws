namespace EA.Iws.Web.Areas.Reports.ViewModels.MissingShipments
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Prsd.Core;

    public class IndexViewModel
    {
        private const int MinYear = 2005;

        [Display(Name = "Year", ResourceType = typeof(IndexViewModelResources))]
        [Required(ErrorMessageResourceName = "YearRequired", ErrorMessageResourceType = typeof(IndexViewModelResources))]
        public int? Year { get; set; }

        public SelectList Years { get; private set; }

        public IndexViewModel()
        {
            Years = new SelectList(Enumerable.Range(MinYear, SystemTime.UtcNow.Year + 1 - MinYear));
        }
    }
}