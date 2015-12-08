namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    internal class InternalUserMapping : EntityTypeConfiguration<InternalUser>
    {
        public InternalUserMapping()
        {
            ToTable("InternalUser", "Person");

            HasRequired(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            Property(x => x.JobTitle).IsRequired().HasMaxLength(256);
            Property(x => x.CompetentAuthority.Value).IsRequired();

            HasRequired(x => x.LocalArea)
                .WithMany()
                .HasForeignKey(x => x.LocalAreaId);

            Property(x => x.Status).IsRequired();
        }
    }
}
