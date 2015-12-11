namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportNotification;
    using Prsd.Core.Helpers;

    internal class FacilityCollectionMapping : EntityTypeConfiguration<FacilityCollection>
    {
        public FacilityCollectionMapping()
        {
            ToTable("FacilityCollection", "ImportNotification");

            HasMany(
                ExpressionHelper.GetPrivatePropertyExpression<FacilityCollection, ICollection<Facility>>(
                    "FacilitiesCollection"))
                .WithRequired()
                .Map(m => m.MapKey("FacilityCollectionId"));
        }
    }
}