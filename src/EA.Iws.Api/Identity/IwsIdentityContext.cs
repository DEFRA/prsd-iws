namespace EA.Iws.Api.Identity
{
    using System.Data.Entity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class IwsIdentityContext : IdentityDbContext<ApplicationUser>
    {
        public IwsIdentityContext()
            : base("name=Iws.DefaultConnection")
        {
            Database.SetInitializer<IwsIdentityContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Identity");
            modelBuilder.Entity<ApplicationUser>()
                .ToTable("AspNetUsers", "Identity")
                .Property(p => p.Id)
                .HasColumnName("Id");
            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
        }
    }
}