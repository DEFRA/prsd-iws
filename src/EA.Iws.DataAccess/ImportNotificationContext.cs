namespace EA.Iws.DataAccess
{
    using System.Data.Entity;
    using Domain.ImportMovement;
    using Domain.ImportNotification;
    using Mappings.Imports;
    using Prsd.Core.Domain;

    public class ImportNotificationContext : ContextBase
    {
        public ImportNotificationContext(IUserContext userContext, IEventDispatcher dispatcher)
            : base(userContext, dispatcher)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.AddFromNamespace(typeof(ImportNotificationMapping).Namespace);
        }

        public virtual DbSet<ImportMovement> ImportMovements { get; set; }

        public virtual DbSet<ImportNotification> ImportNotifications { get; set; }

        public virtual DbSet<Importer> Importers { get; set; } 
    }
}
