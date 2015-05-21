namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    internal class CountryMapping : EntityTypeConfiguration<Country>
    {
        public CountryMapping()
        {
            this.ToTable("Country", "Lookup");

            Property(x => x.Name).IsRequired().HasMaxLength(2048);
            Property(x => x.IsEuropeanUnionMember).IsRequired();
            Property(x => x.IsoAlpha2Code).IsRequired().HasMaxLength(2);
        }
    }
}
