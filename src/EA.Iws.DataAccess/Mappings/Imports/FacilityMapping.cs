namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportNotification;

    internal class FacilityMapping : EntityTypeConfiguration<Facility>
    {
        public FacilityMapping()
        {
            ToTable("Facility", "ImportNotification");

            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(3000);
            Property(x => x.Type).HasColumnName("Type").IsRequired();
            Property(x => x.RegistrationNumber).HasColumnName("RegistrationNumber").IsOptional().HasMaxLength(100);
        }
    }
}