namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    internal class PhoneNumberMapping : ComplexTypeConfiguration<PhoneNumber>
    {
        public PhoneNumberMapping()
        {
            Property(x => x.Value).HasColumnName("Telephone").IsRequired().HasMaxLength(150);
        }
    }
}