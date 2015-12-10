namespace EA.Iws.DataAccess.Mappings.Common
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    internal class OperationCodesMapping : ComplexTypeConfiguration<OperationCode>
    {
        public OperationCodesMapping()
        {
            Property(x => x.Value).HasColumnName("OperationCode").IsRequired();
            Ignore(x => x.DisplayName);
            Ignore(x => x.NotificationType);
        }
    }
}
