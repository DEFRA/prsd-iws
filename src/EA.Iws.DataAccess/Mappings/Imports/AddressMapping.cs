namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportNotification;

    internal class AddressMapping : ComplexTypeConfiguration<Address>
    {
        public AddressMapping()
        {
            Property(x => x.Address1).HasColumnName("Address1").IsRequired().HasMaxLength(1024);
            Property(x => x.Address2).HasColumnName("Address2").HasMaxLength(1024);
            Property(x => x.PostalCode).HasColumnName("PostalCode").HasMaxLength(64);
            Property(x => x.TownOrCity).HasColumnName("TownOrCity").IsRequired().HasMaxLength(1024);
            Property(x => x.CountryId).HasColumnName("CountryId");
        }
    }
}