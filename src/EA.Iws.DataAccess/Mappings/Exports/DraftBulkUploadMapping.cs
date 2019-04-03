namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.Movement.BulkUpload;

    internal class DraftBulkUploadMapping : EntityTypeConfiguration<DraftBulkUpload>
    {
        public DraftBulkUploadMapping()
        {
            ToTable("BulkUpload", "Draft");
        }
    }
}
