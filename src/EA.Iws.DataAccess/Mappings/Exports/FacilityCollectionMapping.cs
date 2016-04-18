namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication;
    using Prsd.Core.Helpers;

    internal class FacilityCollectionMapping : EntityTypeConfiguration<FacilityCollection>
    {
        public FacilityCollectionMapping()
        {
            ToTable("FacilityCollection", "Notification");

            HasMany(
                ExpressionHelper.GetPrivatePropertyExpression<FacilityCollection, ICollection<Facility>>(
                    "FacilitiesCollection"))
                .WithRequired()
                .Map(m => m.MapKey("FacilityCollectionId"));
        }
    }
}