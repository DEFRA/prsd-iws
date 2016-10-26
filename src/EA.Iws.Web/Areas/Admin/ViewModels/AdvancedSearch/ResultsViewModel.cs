namespace EA.Iws.Web.Areas.Admin.ViewModels.AdvancedSearch
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Admin.Search;

    public class ResultsViewModel
    {
        public ExportAdvancedSearchResult[] ExportResults { get; set; }

        public bool HasExportResults
        {
            get { return ExportResults.Any(); }
        }

        public ImportAdvancedSearchResult[] ImportResults { get; set; }

        public bool HasImportResults
        {
            get { return ImportResults.Any(); }
        }
    }
}