namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    internal class AddressMapping : EntityTypeConfiguration<Address>
    {
        public AddressMapping()
        {
            this.ToTable("Address", "Business");

            this.Ignore(address => address.IsUkAddress);
        }
    }
}
