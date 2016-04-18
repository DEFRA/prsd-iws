namespace EA.Iws.DataAccess.Mappings.Common
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    internal class LocalAreaMapping : EntityTypeConfiguration<LocalArea>
    {
        public LocalAreaMapping()
        {
            this.ToTable("LocalArea", "Lookup");
        }
    }
}
