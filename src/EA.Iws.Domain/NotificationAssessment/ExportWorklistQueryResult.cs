namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    public class ExportWorklistQueryResult
    {
        public ExportWorklistSummary[] PagedRows { get; set; }
        public int TotalCount { get; set; }
    }
}
