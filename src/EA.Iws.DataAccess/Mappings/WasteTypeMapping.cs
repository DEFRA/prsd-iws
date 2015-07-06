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
            Property(x => x.OtherWasteTypeDescription).IsOptional().HasMaxLength(256);
            Property(x => x.WoodTypeDescription).IsOptional().HasMaxLength(256);
            Property(x => x.EnergyInformation).IsOptional().HasMaxLength(256);
            Property(x => x.OptionalInformation).IsOptional().HasMaxLength(256);
            Property(x => x.HasAnnex).IsOptional();

            HasMany(
                ExpressionHelper.GetPrivatePropertyExpression<WasteType, ICollection<WasteComposition>>(
                    "WasteCompositionCollection"))
                .WithRequired()
                .Map(m => m.MapKey("WasteTypeId"));

            HasMany(
                ExpressionHelper.GetPrivatePropertyExpression<WasteType, ICollection<WasteAdditionalInformation>>(
                    "WasteAdditionalInformationCollection"))
                .WithRequired()
                .Map(m => m.MapKey("WasteTypeId"));
        }
    }
}
