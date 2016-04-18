namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication;

    internal class ContactMapping : ComplexTypeConfiguration<Contact>
    {
        public ContactMapping()
        {
            Property(x => x.FullName).HasColumnName("FullName").IsRequired().HasMaxLength(2048);
            Property(x => x.Telephone).HasColumnName("Telephone").IsRequired().HasMaxLength(150);
            Property(x => x.Fax).HasColumnName("Fax").HasMaxLength(150);
            Property(x => x.Email).HasColumnName("Email").IsRequired().HasMaxLength(256);
        }
    }
}
