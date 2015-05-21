namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    internal class ContactMapping : ComplexTypeConfiguration<Contact>
    {
        public ContactMapping()
        {
            Property(x => x.FirstName).HasColumnName("FirstName").IsRequired().HasMaxLength(1024);
            Property(x => x.LastName).HasColumnName("LastName").IsRequired().HasMaxLength(1024);
            Property(x => x.Telephone).HasColumnName("Telephone").IsRequired().HasMaxLength(150);
            Property(x => x.Fax).HasColumnName("Fax").HasMaxLength(150);
            Property(x => x.Email).HasColumnName("Email").IsRequired().HasMaxLength(256);
        }
    }
}
