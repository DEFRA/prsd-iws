namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    internal class UserAddressMapping : EntityTypeConfiguration<UserAddress>
    {
        public UserAddressMapping()
        {
            this.ToTable("Address", "Person");

            Property(x => x.Address.Address1).HasColumnName("Address1").IsRequired().HasMaxLength(1024);
            Property(x => x.Address.Address2).HasColumnName("Address2");
            Property(x => x.Address.TownOrCity).HasColumnName("TownOrCity").IsRequired().HasMaxLength(1024);
            Property(x => x.Address.Region).HasColumnName("Region").HasMaxLength(1024);
            Property(x => x.Address.PostalCode).HasColumnName("PostalCode").HasMaxLength(64);
            Property(x => x.Address.Country).HasColumnName("Country").IsRequired().HasMaxLength(1024);
        }
    }
}
