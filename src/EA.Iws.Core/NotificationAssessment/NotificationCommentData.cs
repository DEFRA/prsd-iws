namespace EA.Iws.Core.NotificationAssessment
{
    using System.Collections.Generic;
    using InternalComments;

    public class NotificationCommentData
    {
        public IList<InternalComment> NotificationComments { get; set; }
        public int NumberOfComments { get; set; }

        public NotificationCommentData()
        {
        }
    }
}
