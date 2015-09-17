namespace EA.Iws.Domain.Movement
{
    using NotificationApplication;
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
    }
}
