namespace EA.Iws.Core.ImportNotificationAssessment
{
    using System.Collections.Generic;

    public class ImportWorklistResult
    {
        public IList<ImportWorklistTableData> Results { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}