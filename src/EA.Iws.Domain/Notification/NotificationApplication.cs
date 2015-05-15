namespace EA.Iws.Domain.Notification
{
    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using System.Linq;
    using Prsd.Core.Domain;

    public class NotificationApplication : Entity
    {
        private const string NotificationNumberFormat = "GB 000{0} {1}";

        protected NotificationApplication()
        {
        }

        public NotificationApplication(Guid userId, WasteAction wasteAction, UKCompetentAuthority competentAuthority,
            int notificationNumber)
        {
            UserId = userId;
            WasteAction = wasteAction;
            CompetentAuthority = competentAuthority;
            NotificationNumber = CreateNotificationNumber(notificationNumber);
            ProducersCollection = new List<Producer>();
        }

        public Guid UserId { get; private set; }

        public WasteAction WasteAction { get; private set; }

        public UKCompetentAuthority CompetentAuthority { get; private set; }

        public Exporter Exporter { get; private set; }

        public string NotificationNumber { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public void AddExporter(Exporter exporter)
        {
            Exporter = exporter;
        }

        private string CreateNotificationNumber(int notificationNumber)
        {
            return string.Format(NotificationNumberFormat, CompetentAuthority.Value, notificationNumber.ToString("D6"));
        }

        protected virtual ICollection<Producer> ProducersCollection { get; set; }

        public IEnumerable<Producer> Producers
        {
            get
            {
                // Hack to make it return an IEnumerable otherwise could
                // be cast back to ICollection!
                return ProducersCollection == null ? new Producer[] { } : ProducersCollection.Skip(0);
            }
        }

        public void AddProducer(Producer producer)
        {
            if (ProducersCollection == null)
            {
                ProducersCollection = new List<Producer>();
            }

            ProducersCollection.Add(producer);
        }

        public Importer Importer { get; private set; }

        public void AddImporter(Importer importer)
        {
            Importer = importer;
        }

        public void RemoveProducer(Producer producer)
        {
            if (ProducersCollection == null || !ProducersCollection.Contains(producer))
            {
                throw new InvalidOperationException(String.Format("Unable to remove producer with id {0}", producer.Id));
            }

            ProducersCollection.Remove(producer);
        }
    }
}