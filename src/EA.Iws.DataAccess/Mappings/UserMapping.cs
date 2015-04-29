namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    internal class UserMapping : EntityTypeConfiguration<User>
    {
        public UserMapping()
        {
            ToTable("AspNetUsers", "Identity");
        }
    }
}
