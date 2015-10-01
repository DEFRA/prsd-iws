namespace EA.Iws.DataAccess.Filestore
{
    using System.Data.Entity;
    using Domain.FileStore;

    public class IwsFileStoreContext : DbContext
    {
        public IwsFileStoreContext()
            : base("name=Iws.DefaultConnection")
        {
            Database.SetInitializer<IwsFileStoreContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<File>()
                .ToTable("File", "FileStore");
        }

        public virtual DbSet<File> Files { get; set; } 
    }
}