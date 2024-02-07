namespace EA.Iws.Domain.Finance
{
    using EA.Iws.Core.Notification;
    using EA.Iws.Core.WasteComponentType;
    using EA.Iws.Core.WasteType;
    using System;

    public class PricingFixedFee
    {
        protected PricingFixedFee()
        {
        }

        public Guid Id { get; protected set; }

        public WasteComponentType? WasteComponentTypeId { get; protected set; }

        public WasteCategoryType? WasteCategoryTypeId { get; protected set; }

        public decimal Price { get; protected set; }

        public DateTime ValidFrom { get; private set; }

        public UKCompetentAuthority CompetentAuthority { get; private set; }
    }
}