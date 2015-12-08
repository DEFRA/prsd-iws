namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication;

    internal class ProducerBusinessMapping : ComplexTypeConfiguration<ProducerBusiness>
    {
        public ProducerBusinessMapping()
        {
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(3000);
            Property(x => x.RegistrationNumber).HasColumnName("RegistrationNumber").IsRequired().HasMaxLength(100);
            Property(x => x.AdditionalRegistrationNumber).HasColumnName("AdditionalRegistrationNumber").HasMaxLength(100);
            Property(x => x.OtherDescription).HasColumnName("OtherDescription").HasMaxLength(100);
        }
    }
}
