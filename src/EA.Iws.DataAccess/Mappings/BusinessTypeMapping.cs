namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;
    using Domain.NotificationApplication;

    internal class BusinessTypeMapping : ComplexTypeConfiguration<BusinessType>
    {
        public BusinessTypeMapping()
        {
            Ignore(x => x.DisplayName);
            Property(x => x.Value).HasColumnName("Type").IsRequired();
        }
    }
}