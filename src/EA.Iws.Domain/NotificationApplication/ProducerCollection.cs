namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Prsd.Core.Domain;
    using Prsd.Core.Extensions;

    public class ProducerCollection : Entity
    {
        protected ProducerCollection()
        {
        }

        public ProducerCollection(Guid notificationId)
        {
            NotificationId = notificationId;
            ProducersCollection = new List<Producer>();
        }

        public Guid NotificationId { get; private set; }

        protected virtual ICollection<Producer> ProducersCollection { get; set; }

        public IEnumerable<Producer> Producers
        {
            get { return ProducersCollection.ToSafeIEnumerable(); }
        }

        public Producer AddProducer(ProducerBusiness business, Address address, Contact contact)
        {
            var producer = new Producer(business, address, contact);

            ProducersCollection.Add(producer);
            return producer;
        }

        public Producer GetProducer(Guid producerId)
        {
            var producer = ProducersCollection.SingleOrDefault(p => p.Id == producerId);
            if (producer == null)
            {
                throw new InvalidOperationException(
                    string.Format("Producer with id {0} does not exist on this notification {1}", producerId, Id));
            }
            return producer;
        }

        public void RemoveProducer(Guid producerId)
        {
            var producer = GetProducer(producerId);
            if (producer.IsSiteOfExport && ProducersCollection.Count > 1)
            {
                throw new InvalidOperationException(string.Format("Cannot remove producer with id {0} for notification {1} without making another producer as site of export",
                    producerId, Id));
            }

            ProducersCollection.Remove(producer);
        }

        public void SetProducerAsSiteOfExport(Guid producerId)
        {
            if (ProducersCollection.All(p => p.Id != producerId))
            {
                throw new InvalidOperationException(
                    string.Format("Unable to make producer with id {0} the site of export on this notification {1}", producerId, Id));
            }

            foreach (var prod in ProducersCollection)
            {
                prod.IsSiteOfExport = prod.Id == producerId;
            }
        }
    }
}