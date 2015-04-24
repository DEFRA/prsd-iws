namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    internal class OrganisationMapping : EntityTypeConfiguration<Organisation>
    {
        public OrganisationMapping()
        {
            this.ToTable("Organisation", "Business");

            HasRequired(o => o.Address);
        }
    }
}
