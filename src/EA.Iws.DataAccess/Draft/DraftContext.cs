namespace EA.Iws.DataAccess.Draft
{
    using System.Data.Entity;

    internal class DraftContext : DbContext
    {
        public DraftContext()
            : base("name=Iws.DefaultConnection")
        {
            Database.SetInitializer<DraftContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Import>()
                .ToTable("Import", "Draft");
        }

        public virtual DbSet<Import> Imports { get; set; }
    }
}