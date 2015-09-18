namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Collections.Generic;
    using Core.Shared;
    using MovementOperationReceipt;
    using MovementReceipt;
    using NotificationApplication;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using Prsd.Core.Extensions;
    using MovementReceiptDecision = Core.MovementReceipt.Decision;

    public class Movement : Entity
    {
        protected Movement()
        {
        }

        internal Movement(NotificationApplication notificationApplication, int movementNumber)
        {
            NotificationApplicationId = notificationApplication.Id;
            Number = movementNumber;
            NotificationApplication = notificationApplication;
        }

        public int Number { get; private set; }

        public bool IsActive
        {
            get
            {
                return this.Date.HasValue
                    && this.Date < SystemTime.UtcNow;
            }
        }

        public bool IsReceived
        {
            get
            {
                return this.Receipt != null
                    && this.Receipt.Decision == MovementReceiptDecision.Accepted
                    && this.Receipt.Quantity.HasValue;
            }
        }

        public virtual NotificationApplication NotificationApplication { get; private set; }

        public virtual MovementReceipt Receipt { get; private set; }

        public DateTime? Date { get; private set; }

        public Guid NotificationApplicationId { get; private set; }

        public decimal? Quantity { get; private set; }

        public int? NumberOfPackages { get; private set; }

        public ShipmentQuantityUnits? Units { get; private set; }

        protected virtual ICollection<PackagingInfo> PackagingInfosCollection { get; set; }

        protected virtual ICollection<MovementCarrier> MovementCarriersCollection { get; set; }

        public IEnumerable<PackagingInfo> PackagingInfos
        {
            get { return PackagingInfosCollection.ToSafeIEnumerable(); }
        }

        public IEnumerable<MovementCarrier> MovementCarriers
        {
            get { return MovementCarriersCollection.ToSafeIEnumerable(); }
        }

        public void UpdateDate(DateTime date)
        {
            if (date >= NotificationApplication.ShipmentInfo.FirstDate
                && date <= NotificationApplication.ShipmentInfo.LastDate)
            {
                this.Date = date;
            }
            else
            {
                throw new InvalidOperationException(
                    "The date is not within the shipment date range for this notification " + date);
            }
        }

        public void SetQuantity(ShipmentQuantity shipmentQuantity)
        {
            Quantity = shipmentQuantity.Quantity;
            Units = shipmentQuantity.Units;
        }

        public void SetPackagingInfos(IEnumerable<PackagingInfo> packagingInfos)
        {
            PackagingInfosCollection.Clear();

            foreach (var packagingInfo in packagingInfos)
            {
                PackagingInfosCollection.Add(packagingInfo);
            }
        }

        public void SetNumberOfPackages(int number)
        {
            Guard.ArgumentNotZeroOrNegative(() => number, number);

            NumberOfPackages = number;
        }

        public void SetMovementCarriers(IEnumerable<MovementCarrier> movementCarriers)
        {
            MovementCarriersCollection.Clear();

            foreach (var movementCarrier in movementCarriers)
            {
                MovementCarriersCollection.Add(movementCarrier);
            }
        }

        public MovementReceipt Receive(DateTime dateReceived)
        {
            if (!this.Date.HasValue || dateReceived < this.Date)
            {
                throw new InvalidOperationException("Cannot receive a movement this is not active.");
            }

            if (this.Receipt == null)
            {
                this.Receipt = new MovementReceipt(dateReceived);
            }
            else
            {
                this.Receipt.Date = dateReceived;
            }

            return this.Receipt;
        }

        public void Accept()
        {
            if (this.Receipt == null)
            {
                throw new InvalidOperationException("Cannot accept a movement that has not been received.");
            }

            this.Receipt.Decision = MovementReceiptDecision.Accepted;
        }

        public void Reject(string reason)
        {
            if (this.Receipt == null)
            {
                throw new InvalidOperationException("Cannot reject a movement that has not been received.");
            }

            this.Receipt.Decision = MovementReceiptDecision.Rejected;
            this.Receipt.RejectReason = reason;
        }

        public void CompleteMovement(DateTime dateComplete)
        {
            if (!this.IsReceived)
            {
                throw new InvalidOperationException("Cannot complete a movement that has not been received.");
            }

            this.Receipt.OperationReceipt = new MovementOperationReceipt(dateComplete);
        }
    }
}
