namespace EA.Iws.Core.Admin.Search
{
    using System.Collections.Generic;

    public class AdvancedSearchResult
    {
        public IEnumerable<ExportAdvancedSearchResult> ExportResults { get; set; }

        public IEnumerable<ImportAdvancedSearchResult> ImportResults { get; set; }
    }
}