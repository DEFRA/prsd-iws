namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportNotification;

    internal class ContactMapping : ComplexTypeConfiguration<Contact>
    {
        public ContactMapping()
        {
            Property(x => x.Name).HasColumnName("ContactName").IsRequired().HasMaxLength(1024);
        }
    }
}