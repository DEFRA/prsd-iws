namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.Movement.BulkUpload;

    internal class DraftPackagingInfoMapping : EntityTypeConfiguration<DraftPackagingInfo>
    {
        public DraftPackagingInfoMapping()
        {
            ToTable("PackagingInfo", "Draft");
        }
    }
}
