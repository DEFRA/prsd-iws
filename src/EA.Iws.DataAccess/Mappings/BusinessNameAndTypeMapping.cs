namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    internal class BusinessNameAndTypeMapping : ComplexTypeConfiguration<Business>
    {
        public BusinessNameAndTypeMapping()
        {
            Property(x => x.Name).HasColumnName("Name");
            Property(x => x.Type).HasColumnName("Type");
            Property(x => x.RegistrationNumber).HasColumnName("RegistrationNumber");
            Property(x => x.AdditionalRegistrationNumber).HasColumnName("AdditionalRegistrationNumber");
        }
    }
}
