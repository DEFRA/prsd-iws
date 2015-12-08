namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    internal class UserMapping : EntityTypeConfiguration<User>
    {
        public UserMapping()
        {
            ToTable("AspNetUsers", "Identity");

            Property(x => x.FirstName).IsRequired().HasMaxLength(256);
            Property(x => x.Surname).IsRequired().HasMaxLength(256);
            Property(x => x.Email).IsRequired().HasMaxLength(256);
            Property(x => x.PhoneNumber).IsOptional();
        }
    }
}
