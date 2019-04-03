namespace EA.Iws.Domain.Movement
{
    using System;
    using NotificationApplication;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class MovementCarrier : Entity
    {
        protected MovementCarrier()
        {
        }

        public MovementCarrier(int order, Carrier carrier)
        {
            Order = order;
            Carrier = carrier;
        }

        public virtual Carrier Carrier { get; private set; }

        public int Order { get; private set; }

        public Guid MovementId { get; private set; }

        public Guid CarrierId { get; private set; }

        public DateTime CreatedOnDate { get; private set; }

        public MovementCarrier(Guid movementId, Guid carrierId, int order)
        {
            Guard.ArgumentNotZeroOrNegative(() => order, order);

            MovementId = movementId;
            CarrierId = carrierId;
            Order = order;
            CreatedOnDate = SystemTime.UtcNow;
        }
    }
}
