namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    internal class BusinessNameAndTypeMapping : ComplexTypeConfiguration<BusinessNameAndType>
    {
        public BusinessNameAndTypeMapping()
        {
            Property(x => x.Name).HasColumnName("Name");
            Property(x => x.Type).HasColumnName("Type");
            Property(x => x.CompaniesHouseNumber).HasColumnName("CompaniesHouseNumber");
            Property(x => x.RegistrationNumber1).HasColumnName("RegistrationNumber1");
            Property(x => x.RegistrationNumber2).HasColumnName("RegistrationNumber2");
        }
    }
}
