namespace EA.Iws.Core.ImportNotificationAssessment
{
    using System.Collections.Generic;
    using InternalComments;

    public class ImportNotificationCommentsData
    {
        public IList<InternalComment> NotificationComments { get; set; }
        public int NumberOfComments { get; set; }
        public int NumberOfFilteredComments { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public ImportNotificationCommentsData()
        {
        }
    }
}
