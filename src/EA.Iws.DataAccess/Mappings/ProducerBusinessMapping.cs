namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    internal class ProducerBusinessMapping : ComplexTypeConfiguration<ProducerBusiness>
    {
        public ProducerBusinessMapping()
        {
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(3000);
            Property(x => x.RegistrationNumber).HasColumnName("RegistrationNumber").IsRequired().HasMaxLength(64);
            Property(x => x.AdditionalRegistrationNumber).HasColumnName("AdditionalRegistrationNumber").HasMaxLength(64);
            Property(x => x.OtherDescription).HasColumnName("OtherDescription").HasMaxLength(100);
        }
    }
}
