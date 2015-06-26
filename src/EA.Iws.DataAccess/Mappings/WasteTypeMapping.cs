namespace EA.Iws.DataAccess.Mappings
{
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using Domain.Notification;
    using Prsd.Core.Helpers;

    internal class WasteTypeMapping : EntityTypeConfiguration<WasteType>
    {
        public WasteTypeMapping()
        {
            ToTable("WasteType", "Business");

            Property(x => x.ChemicalCompositionName).IsOptional().HasMaxLength(120);
            Property(x => x.ChemicalCompositionDescription).IsOptional().HasMaxLength(1024);

            HasMany(
                ExpressionHelper.GetPrivatePropertyExpression<WasteType, ICollection<WasteComposition>>(
                    "WasteCompositionCollection"))
                .WithRequired()
                .Map(m => m.MapKey("WasteTypeId"));
        }
    }
}
