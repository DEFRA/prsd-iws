namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication;

    internal class ChemicalCompositionTypeMapping : ComplexTypeConfiguration<ChemicalComposition>
    {
        public ChemicalCompositionTypeMapping()
        {
            Ignore(x => x.DisplayName);
            Property(x => x.Value)
                .HasColumnName("ChemicalCompositionType");
        }
    }
}