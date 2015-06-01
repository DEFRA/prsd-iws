namespace EA.Iws.Domain.Notification
{
    using System;
    using System.Collections.Generic;
    using Prsd.Core.Domain;
    using Prsd.Core.Extensions;

    public partial class NotificationApplication : Entity
    {
        private const string NotificationNumberFormat = "GB 000{0} {1}";

        protected NotificationApplication()
        {
        }

        public NotificationApplication(Guid userId, NotificationType notificationType,
            UKCompetentAuthority competentAuthority, int notificationNumber)
        {
            UserId = userId;
            NotificationType = notificationType;
            CompetentAuthority = competentAuthority;
            NotificationNumber = CreateNotificationNumber(notificationNumber);

            ProducersCollection = new List<Producer>();
            FacilitiesCollection = new List<Facility>();
            CarriersCollection = new List<Carrier>();
        }

        protected virtual ICollection<Producer> ProducersCollection { get; set; }

        protected virtual ICollection<Facility> FacilitiesCollection { get; set; }

        protected virtual ICollection<Carrier> CarriersCollection { get; set; }

        public Guid UserId { get; private set; }

        public NotificationType NotificationType { get; private set; }

        public UKCompetentAuthority CompetentAuthority { get; private set; }

        private bool? isPreconsented;
        public bool? IsPreconsentedRecoveryFacility
        {
            get
            {
                return isPreconsented;
            }
            set
            {
                if (NotificationType != NotificationType.Recovery)
                {
                    throw new InvalidOperationException(String.Format(
                        "Facility type is not recovery for notification: {0}", Id));
                }

                isPreconsented = value;
            }
        }

        public virtual Exporter Exporter { get; private set; }

        public virtual Importer Importer { get; private set; }

        public string NotificationNumber { get; private set; }

        public virtual ShipmentInfo ShipmentInfo { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public IEnumerable<Producer> Producers
        {
            get { return ProducersCollection.ToSafeIEnumerable(); }
        }

        public IEnumerable<Facility> Facilities
        {
            get { return FacilitiesCollection.ToSafeIEnumerable(); }
        }

        public IEnumerable<Carrier> Carriers
        {
            get { return CarriersCollection.ToSafeIEnumerable(); }
        }

        private string CreateNotificationNumber(int notificationNumber)
        {
            return string.Format(NotificationNumberFormat, CompetentAuthority.Value, notificationNumber.ToString("D6"));
        }
    }
}