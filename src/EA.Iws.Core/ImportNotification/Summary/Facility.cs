namespace EA.Iws.Core.ImportNotification.Summary
{
    using EA.Iws.Core.ImportNotificationAssessment;
    using EA.Iws.Core.Notification;
    using Shared;
    using System;

    public class Facility
    {
        public Address Address { get; set; }

        public Contact Contact { get; set; }

        public BusinessType? BusinessType { get; set; }

        public string RegistrationNumber { get; set; }

        public string Name { get; set; }

        public bool IsActualSite { get; set; }

        public Guid NotificationId { get; set; }

        public UKCompetentAuthority CompetentAuthority { get; set; }

        public ImportNotificationStatus NotificationStatus { get; set; }
    }
}
