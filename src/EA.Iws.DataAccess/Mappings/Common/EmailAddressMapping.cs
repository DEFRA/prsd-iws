namespace EA.Iws.DataAccess.Mappings.Common
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    internal class EmailAddressMapping : ComplexTypeConfiguration<EmailAddress>
    {
        public EmailAddressMapping()
        {
            Property(x => x.Value).HasColumnName("Email").IsRequired().HasMaxLength(256);
        }
    }
}