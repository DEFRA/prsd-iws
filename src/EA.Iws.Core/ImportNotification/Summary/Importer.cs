namespace EA.Iws.Core.ImportNotification.Summary
{
    using EA.Iws.Core.ImportNotificationAssessment;
    using EA.Iws.Core.Notification;
    using Shared;
    using System;

    public class Importer
    {
        public Address Address { get; set; }

        public Contact Contact { get; set; }

        public BusinessType? BusinessType { get; set; }

        public string Name { get; set; }

        public string RegistrationNumber { get; set; }

        public Guid NotificationId { get; set; }

        public UKCompetentAuthority CompetentAuthority { get; set; }

        public ImportNotificationStatus NotificationStatus { get; set; }

        public bool IsEmpty()
        {
            return Address.IsEmpty() &&
                   Contact.IsEmpty() &&
                   !BusinessType.HasValue &&
                   string.IsNullOrWhiteSpace(Name) &&
                   string.IsNullOrWhiteSpace(RegistrationNumber);
        }
    }
}
