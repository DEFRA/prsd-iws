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
            CarriersCollection = new List<Carrier>();
        }

        public Guid UserId { get; private set; }

        public NotificationType NotificationType { get; private set; }

        public UKCompetentAuthority CompetentAuthority { get; private set; }

        public virtual Exporter Exporter { get; private set; }

        public bool HasExporter
        {
            get { return Exporter != null; }
        }

        public bool HasShipmentInfo
        {
            get { return ShipmentInfo != null; }
        }

        public virtual ShipmentInfo ShipmentInfo { get; private set; }

        public string NotificationNumber { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public bool IsSpecialHandling { get; set; }

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

        public bool HasImporter
        {
            get { return Importer != null; }
        }

        private string CreateNotificationNumber(int notificationNumber)
        {
            return string.Format(NotificationNumberFormat, CompetentAuthority.Value, notificationNumber.ToString("D6"));
        }

        protected virtual ICollection<Carrier> CarriersCollection { get; set; }

        public IEnumerable<Carrier> Carriers
        {
            get
            {
                return CarriersCollection.ToSafeIEnumerable();
            }
        }

        public void AddExporter(Business business, Address address, Contact contact)
        {
            if (Exporter != null)
            {
                throw new InvalidOperationException("Exporter already exists, can't add another.");
            }

            Exporter = new Exporter(business, address, contact);
        }

        public void RemoveExporter()
        {
            Exporter = null;
        }

        public void AddProducer(Producer producer)
        {
            ProducersCollection.Add(producer);
        }

        public void RemoveProducer(Guid producerId)
        {
            var producer = ProducersCollection.SingleOrDefault(p => p.Id == producerId);
            if (producer == null)
            {
                throw new InvalidOperationException(string.Format("Unable to remove producer with id {0}", producerId));
            }

            ProducersCollection.Remove(producer);
        }

        public void AddFacility(Facility facility)
        {
            FacilitiesCollection.Add(facility);
        }

        public void AddImporter(Business business, Address address, Contact contact)
        {
            if (Importer != null)
            {
                throw new InvalidOperationException("Importer already exists, can't add another.");
            }

            Importer = new Importer(business, address, contact);
        }

        public void RemoveImporter()
        {
            Importer = null;
        }

        public void SetAsSiteOfExport(Guid producerId)
        {
            if (ProducersCollection.All(p => p.Id != producerId))
            {
                throw new InvalidOperationException(
                    string.Format("Unable to make producer with id {0} the site of export", producerId));
            }

            foreach (var prod in ProducersCollection)
            {
                prod.IsSiteOfExport = prod.Id == producerId;
            }
        }
        public Carrier AddCarrier(Business business, Address address, Contact contact)
        {
            var carrier = new Carrier(business, address, contact);
            CarriersCollection.Add(carrier);
            return carrier;
        }
        
        public void AddShippingInfo(DateTime startDate, DateTime endDate, int numberOfShipments, decimal quantity, ShipmentQuantityUnits unit)
        {
            if (ShipmentInfo != null)
            {
                throw new InvalidOperationException(
                    String.Format("Cannot add shipment info to notification: {0} if it already exists", Id));
            }

            ShipmentInfo = new ShipmentInfo(numberOfShipments, quantity, unit, startDate, endDate);
        }
    }
}