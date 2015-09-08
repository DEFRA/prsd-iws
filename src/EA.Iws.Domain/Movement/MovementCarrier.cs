namespace EA.Iws.Domain.Movement
{
    using EA.Iws.Domain.NotificationApplication;
    using EA.Prsd.Core.Domain;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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
