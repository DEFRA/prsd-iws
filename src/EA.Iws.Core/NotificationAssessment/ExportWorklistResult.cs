namespace EA.Iws.Core.NotificationAssessment
{
    using System.Collections.Generic;

    public class ExportWorklistResult
    {
        public IList<ExportWorklistTableData> Results { get; set; }

        public int TotalCount { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public ExportWorklistResult()
        {
            Results = new List<ExportWorklistTableData>();
        }
    }
}