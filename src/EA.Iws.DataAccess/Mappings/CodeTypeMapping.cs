namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.Notification;

    internal class CodeTypeMapping : ComplexTypeConfiguration<CodeType>
    {
        public CodeTypeMapping()
        {
            Property(x => x.Value).HasColumnName("CodeType").IsRequired();
            Ignore(x => x.DisplayName);
        }
    }
}