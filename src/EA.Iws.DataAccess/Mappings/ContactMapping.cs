namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    internal class ContactMapping : EntityTypeConfiguration<Contact>
    {
        public ContactMapping()
        {
            this.ToTable("Contact", "Business");
        }
    }
}
