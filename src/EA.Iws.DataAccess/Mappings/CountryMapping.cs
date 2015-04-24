namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    internal class CountryMapping : EntityTypeConfiguration<Country>
    {
        public CountryMapping()
        {
            this.ToTable("Country", "Lookup");
        }
    }
}
