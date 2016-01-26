namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication;
    using Prsd.Core.Helpers;

    internal class CarrierCollectionMapping : EntityTypeConfiguration<CarrierCollection>
    {
        public CarrierCollectionMapping()
        {
            ToTable("CarrierCollection", "Notification");

            HasMany(
                ExpressionHelper.GetPrivatePropertyExpression<CarrierCollection, ICollection<Carrier>>(
                    "CarriersCollection"))
                .WithRequired()
                .Map(m => m.MapKey("CarrierCollectionId"));
        }
    }
}