namespace EA.Iws.Core.ImportNotification.Summary
{
    using EA.Iws.Core.ImportNotificationAssessment;
    using EA.Iws.Core.Notification;
    using EA.Iws.Core.Shared;
    using System;

    public class Producer
    {
        public string Name { get; set; }

        public Contact Contact { get; set; }

        public Address Address { get; set; }

        public bool AreMultiple { get; set; }

        public BusinessType BusinessType { get; set; }

        public string RegistrationNumber { get; set; }

        public Guid NotificationId { get; set; }

        public UKCompetentAuthority CompetentAuthority { get; set; }

        public ImportNotificationStatus NotificationStatus { get; set; }

        public bool IsEmpty()
        {
            return Address.IsEmpty()
                && Contact.IsEmpty()
                && string.IsNullOrWhiteSpace(Name);
        }
    }
}
