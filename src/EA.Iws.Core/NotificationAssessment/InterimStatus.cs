namespace EA.Iws.Core.NotificationAssessment
{
    using System;

    public class InterimStatus
    {
        public Guid NotificationId { get; set; }

        public NotificationStatus NotificationStatus { get; set; }

        public bool? IsInterim { get; set; }
    }
}