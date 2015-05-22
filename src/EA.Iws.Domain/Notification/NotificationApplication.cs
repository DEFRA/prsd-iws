namespace EA.Iws.Domain.Notification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Prsd.Core.Domain;
    using Prsd.Core.Extensions;

    public class NotificationApplication : Entity
    {
        private const string NotificationNumberFormat = "GB 000{0} {1}";

        protected NotificationApplication()
        {
        }

        public NotificationApplication(Guid userId, NotificationType notificationType,
            UKCompetentAuthority competentAuthority,
            int notificationNumber)
        {
            UserId = userId;
            NotificationType = notificationType;
            CompetentAuthority = competentAuthority;
            NotificationNumber = CreateNotificationNumber(notificationNumber);
            ProducersCollection = new List<Producer>();
            FacilitiesCollection = new List<Facility>();
        }

        public Guid UserId { get; private set; }

        public NotificationType NotificationType { get; private set; }

        public UKCompetentAuthority CompetentAuthority { get; private set; }

        public virtual Exporter Exporter { get; private set; }

        public string NotificationNumber { get; private set; }

        public DateTime CreatedDate { get; private set; }

        protected virtual ICollection<Producer> ProducersCollection { get; set; }

        public IEnumerable<Producer> Producers
        {
            get { return ProducersCollection.ToSafeIEnumerable(); }
        }

        protected virtual ICollection<Facility> FacilitiesCollection { get; set; }

        public IEnumerable<Facility> Facilities
        {
            get { return FacilitiesCollection.ToSafeIEnumerable(); }
        }

        public virtual Importer Importer { get; private set; }

        public void AddExporter(Exporter exporter)
        {
            Exporter = exporter;
        }

        private string CreateNotificationNumber(int notificationNumber)
        {
            return string.Format(NotificationNumberFormat, CompetentAuthority.Value, notificationNumber.ToString("D6"));
        }

        public void AddProducer(Producer producer)
        {
            ProducersCollection.Add(producer);
        }

        public void RemoveProducer(Producer producer)
        {
            if (!ProducersCollection.Contains(producer))
            {
                throw new InvalidOperationException(String.Format("Unable to remove producer with id {0}", producer.Id));
            }

            ProducersCollection.Remove(producer);
        }

        public void AddFacility(Facility facility)
        {
            FacilitiesCollection.Add(facility);
        }

        public void AddImporter(Importer importer)
        {
            Importer = importer;
        }

        public void SetAsSiteOfExport(Guid producerId)
        {
            if (ProducersCollection.All(p => p.Id != producerId))
            {
                throw new InvalidOperationException(
                    String.Format("Unable to make producer with id {0} the site of export", producerId));
            }

            foreach (var prod in ProducersCollection)
            {
                prod.IsSiteOfExport = prod.Id == producerId;
            }
        }
    }
}