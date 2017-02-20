namespace EA.Iws.DataAccess
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Threading;
    using System.Threading.Tasks;
    using Mappings.Common;
    using Prsd.Core.DataAccess.Extensions;
    using Prsd.Core.Domain;

    public class ContextBase : DbContext
    {
        protected readonly IUserContext UserContext;
        protected readonly IEventDispatcher Dispatcher;

        public ContextBase(IUserContext userContext, IEventDispatcher dispatcher)
            : base("name=Iws.DefaultConnection")
        {
            this.UserContext = userContext;
            this.Dispatcher = dispatcher;

            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 90;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var assembly = typeof(IwsContext).Assembly;
            var coreAssembly = typeof(AuditorExtensions).Assembly;

            modelBuilder.Conventions.AddFromAssembly(assembly);
            modelBuilder.Conventions.AddFromAssembly(coreAssembly);

            modelBuilder.Configurations.AddFromAssembly(coreAssembly);

            modelBuilder.Configurations.AddFromNamespace(typeof(EmailAddressMapping).Namespace);
        }

        public override int SaveChanges()
        {
            this.SetEntityId();
            this.DeleteRemovedRelationships();
            this.AuditChanges(UserContext.UserId);

            int result = base.SaveChanges();

            Task.Run(() => this.DispatchEvents(Dispatcher)).Wait();

            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            this.SetEntityId();
            this.DeleteRemovedRelationships();
            this.AuditChanges(UserContext.UserId);

            int result = await base.SaveChangesAsync(cancellationToken);

            await this.DispatchEvents(Dispatcher);

            return result;
        }

        public void DeleteOnCommit(Entity entity)
        {
            Entry(entity).State = EntityState.Deleted;
        }
    }
}
