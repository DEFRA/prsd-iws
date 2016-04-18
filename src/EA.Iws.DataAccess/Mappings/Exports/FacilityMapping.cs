namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication;

    internal class FacilityMapping : EntityTypeConfiguration<Facility>
    {
        public FacilityMapping()
        {
            ToTable("Facility", "Notification");

            Property(x => x.IsActualSiteOfTreatment).IsRequired();
        }
    }
}
