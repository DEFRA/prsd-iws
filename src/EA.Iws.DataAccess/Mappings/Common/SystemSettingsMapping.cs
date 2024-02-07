namespace EA.Iws.DataAccess.Mappings.Common
{
    using EA.Iws.Domain;
    using System.Data.Entity.ModelConfiguration;

    internal class SystemSettingsMapping : EntityTypeConfiguration<SystemSetting>
    {
        public SystemSettingsMapping()
        {
            ToTable("SystemSettings", "Lookup");
        }
    }
}