namespace EA.Iws.Domain.Finance
{
    using System;
    using Core.Notification;

    public class PricingStructure
    {
        protected PricingStructure()
        {
        }    
        
        public Guid Id { get; protected set; }

        public UKCompetentAuthority CompetentAuthority { get; protected set; }

        public virtual ShipmentQuantityRange ShipmentQuantityRange { get; protected set; }

        public Guid ShipmentQuantityRangeId { get; protected set; }

        public virtual Activity Activity { get; protected set; }

        public Guid ActivityId { get; protected set; }

        public decimal Price { get; protected set; }
    }
}