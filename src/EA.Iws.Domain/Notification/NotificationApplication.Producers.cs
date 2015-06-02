namespace EA.Iws.Domain.Notification
{
    using System;
    using System.Linq;

    public partial class NotificationApplication
    {
        public Producer AddProducer(Business business, Address address, Contact contact)
        {
            var producer = new Producer(business, address, contact);

            if (!ProducersCollection.Any())
            {
                producer.IsSiteOfExport = true;
            }

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