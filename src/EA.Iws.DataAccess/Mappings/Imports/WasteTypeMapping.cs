namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportNotification;
    using Domain.ImportNotification.WasteCodes;
    using Prsd.Core.Helpers;

    internal class WasteTypeMapping : EntityTypeConfiguration<WasteType>
    {
        public WasteTypeMapping()
        {
            ToTable("WasteType", "ImportNotification");

            Property(x => x.Name).HasMaxLength(256);

            HasMany(
                ExpressionHelper.GetPrivatePropertyExpression<WasteType, ICollection<WasteTypeWasteCode>>(
                        "WasteCodesCollection"))
                    .WithRequired()
                    .Map(m => m.MapKey("WasteTypeId"));
        }
    }
}