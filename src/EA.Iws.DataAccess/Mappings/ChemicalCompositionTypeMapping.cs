namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;

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