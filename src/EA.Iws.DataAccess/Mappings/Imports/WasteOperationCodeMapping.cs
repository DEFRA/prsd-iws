namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportNotification;

    internal class WasteOperationCodeMapping : EntityTypeConfiguration<WasteOperationCode>
    {
        public WasteOperationCodeMapping()
        {
            ToTable("OperationCodes", "ImportNotification");
        }
    }
}