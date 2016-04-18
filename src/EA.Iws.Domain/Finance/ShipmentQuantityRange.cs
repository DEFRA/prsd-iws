namespace EA.Iws.Domain.Finance
{
    using System;

    public class ShipmentQuantityRange
    {
        public Guid Id { get; protected set; }

        public int RangeFrom { get; protected set; }

        public int? RangeTo { get; protected set; }

        protected ShipmentQuantityRange()
        {
        }
    }
}