namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportNotification;

    internal class TransitStateMapping : EntityTypeConfiguration<TransitState>
    {
        public TransitStateMapping()
        {
            ToTable("TransitState", "ImportNotification");
        }
    }
}