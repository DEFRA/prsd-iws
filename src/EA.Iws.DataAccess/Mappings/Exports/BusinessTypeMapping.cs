namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
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