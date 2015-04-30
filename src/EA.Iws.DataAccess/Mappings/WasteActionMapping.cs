namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.Notification;

    internal class WasteActionMapping : ComplexTypeConfiguration<WasteAction>
    {
        public WasteActionMapping()
        {
            Ignore(x => x.DisplayName);
            Property(x => x.Value)
                .HasColumnName("WasteAction");
        }
    }
}