namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.TransportRoute;

    internal class EntryExitCustomsSelectionsMapping : EntityTypeConfiguration<EntryExitCustomsSelection>
    {
        public EntryExitCustomsSelectionsMapping()
        {
            this.ToTable("EntryExitCustomsSelection", "Notification");
        }
    }
}
