namespace EA.Iws.Core.Exporters
{
    using EA.Iws.Core.Notification;
    using EA.Iws.Core.NotificationAssessment;
    using Shared;
    using System;

    public class ExporterData
    {
        public Guid Id { get; set; }

        public bool IsUkBased { get; set; }

        public bool HasExporter { get; set; }

        public BusinessInfoData Business { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }

        public Guid NotificationId { get; set; }

        public UKCompetentAuthority CompetentAuthority { get; set; }

        public NotificationStatus NotificationStatus { get; set; }

        public ExporterData()
        {
            Address = new AddressData();

            Contact = new ContactData();

            Business = new BusinessInfoData();
        }
    }
}
