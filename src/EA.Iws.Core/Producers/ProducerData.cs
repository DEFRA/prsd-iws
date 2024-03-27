namespace EA.Iws.Core.Producers
{
    using System;
    using EA.Iws.Core.Notification;
    using EA.Iws.Core.NotificationAssessment;
    using Shared;

    public class ProducerData
    {
        public Guid Id { get; set; }

        public bool IsSiteOfExport { get; set; }

        public BusinessInfoData Business { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }

        public Guid NotificationId { get; set; }

        public UKCompetentAuthority CompetentAuthority { get; set; }

        public NotificationStatus NotificationStatus { get; set; }
    }
}