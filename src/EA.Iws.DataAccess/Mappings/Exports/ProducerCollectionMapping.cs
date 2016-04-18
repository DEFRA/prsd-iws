namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication;
    using Prsd.Core.Helpers;

    internal class ProducerCollectionMapping : EntityTypeConfiguration<ProducerCollection>
    {
        public ProducerCollectionMapping()
        {
            ToTable("ProducerCollection", "Notification");

            HasMany(
                ExpressionHelper.GetPrivatePropertyExpression<ProducerCollection, ICollection<Producer>>(
                    "ProducersCollection"))
                .WithRequired()
                .Map(m => m.MapKey("ProducerCollectionId"));
        }
    }
}