namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    internal class AddressMapping : ComplexTypeConfiguration<Address>
    {
        public AddressMapping()
        {
            Property(x => x.Building).HasColumnName("Building").IsRequired().HasMaxLength(1024);
            Property(x => x.Address1).HasColumnName("Address1").IsRequired().HasMaxLength(1024);
            Property(x => x.Address2).HasColumnName("Address2");
            Property(x => x.TownOrCity).HasColumnName("TownOrCity").IsRequired().HasMaxLength(1024);
            Property(x => x.Region).HasColumnName("Region").HasMaxLength(1024);
            Property(x => x.PostalCode).HasColumnName("PostalCode").IsRequired().HasMaxLength(64);
            Property(x => x.Country).HasColumnName("Country").IsRequired().HasMaxLength(1024);

            this.Ignore(address => address.IsUkAddress);
        }
    }
}
