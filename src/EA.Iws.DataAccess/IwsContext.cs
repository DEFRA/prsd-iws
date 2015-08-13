namespace EA.Iws.DataAccess
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Core;
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Core.Objects.DataClasses;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Security;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain;
    using Domain.FinancialGuarantee;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using Domain.TransportRoute;
    using Prsd.Core.DataAccess.Extensions;
    using Prsd.Core.Domain;
    using Prsd.Core.Domain.Auditing;

    public class IwsContext : DbContext
    {
        private readonly IUserContext userContext;
        private readonly IEventDispatcher dispatcher;

        public IwsContext(IUserContext userContext, IEventDispatcher dispatcher)
            : base("name=Iws.DefaultConnection")
        {
            this.userContext = userContext;
            this.dispatcher = dispatcher;
            Database.SetInitializer<IwsContext>(null);
        }

        public virtual DbSet<AuditLog> AuditLogs { get; set; }

        public virtual DbSet<Country> Countries { get; set; }

        public virtual DbSet<Organisation> Organisations { get; set; }

        public virtual DbSet<NotificationApplication> NotificationApplications { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<CompetentAuthority> CompetentAuthorities { get; set; }

        public virtual DbSet<EntryOrExitPoint> EntryOrExitPoints { get; set; }

        public virtual DbSet<WasteCode> WasteCodes { get; set; }

        public virtual DbSet<PricingStructure> PricingStructures { get; set; }

        public virtual DbSet<NotificationAssessment> NotificationAssessments { get; set; }

        public virtual DbSet<NotificationDecision> NotificationDecisions { get; set; }

        public virtual DbSet<NotificationDates> NotificationDates { get; set; }

        public virtual DbSet<FinancialGuarantee> FinancialGuarantees { get; set; }

        public virtual DbSet<BankHoliday> BankHolidays { get; set; }

        public virtual DbSet<UnitedKingdomCompetentAuthority> UnitedKingdomCompetentAuthorities { get; set; }

        public virtual DbSet<LocalArea> LocalAreas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var assembly = typeof(IwsContext).Assembly;
            var coreAssembly = typeof(AuditorExtensions).Assembly;

            modelBuilder.Conventions.AddFromAssembly(assembly);
            modelBuilder.Configurations.AddFromAssembly(assembly);

            modelBuilder.Conventions.AddFromAssembly(coreAssembly);
            modelBuilder.Configurations.AddFromAssembly(coreAssembly);
        }

        public override int SaveChanges()
        {
            this.SetEntityId();
            DeleteRemovedRelationships(this);
            this.AuditChanges(userContext.UserId);

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            this.SetEntityId();
            DeleteRemovedRelationships(this);
            this.AuditChanges(userContext.UserId);

            int result = await base.SaveChangesAsync(cancellationToken);

            await this.DispatchEvents(dispatcher);

            return result;
        }

        public void DeleteOnCommit(Entity entity)
        {
            Entry(entity).State = EntityState.Deleted;
        }

        public async Task<NotificationApplication> GetNotificationApplication(Guid notificationId)
        {
            var notification = await NotificationApplications.SingleAsync(n => n.Id == notificationId);
            if (notification.UserId != userContext.UserId)
            {
                throw new SecurityException(string.Format("Access denied to this notification {0} for user {1}",
                    notificationId, userContext.UserId));
            }
            return notification;
        }

        public static void DeleteRemovedRelationships(DbContext context)
        {
            var objectContext = ((IObjectContextAdapter)context).ObjectContext;

            var deletedRelationships = objectContext.ObjectStateManager
                .GetObjectStateEntries(EntityState.Deleted)
                .Where(e => e.IsRelationship);

            foreach (var relationEntry in deletedRelationships)
            {
                var entry = GetEntityEntryFromRelation(objectContext, relationEntry, 1);

                // Find representation of the relation 
                var relatedEnd = GetRelatedEnd(entry, relationEntry);

                if (!SkipDeletion(relatedEnd.RelationshipSet.ElementType))
                {
                    if (IsDomainEntity(entry) && entry.State != EntityState.Deleted)
                    {
                        objectContext.DeleteObject(entry.Entity);
                    }
                }
            }
        }

        private static IRelatedEnd GetRelatedEnd(ObjectStateEntry entry, ObjectStateEntry relationEntry)
        {
            return entry.RelationshipManager
                .GetAllRelatedEnds()
                .First(r => r.RelationshipSet == relationEntry.EntitySet);
        }

        private static bool IsDomainEntity(ObjectStateEntry entry)
        {
            return typeof(Entity).IsAssignableFrom(ObjectContext.GetObjectType(entry.Entity.GetType()));
        }

        private static ObjectStateEntry GetEntityEntryFromRelation(ObjectContext context, ObjectStateEntry relationEntry,
            int index)
        {
            var firstKey = (EntityKey)relationEntry.OriginalValues[index];
            var entry = context.ObjectStateManager.GetObjectStateEntry(firstKey);
            if (entry.Entity == null)
            {
                // This hilariously populates the Entity if it was null...
                context.GetObjectByKey(firstKey);
            }
            return entry;
        }

        private static bool SkipDeletion(RelationshipType relationshipType)
        {
            return
                // Many-to-many
                relationshipType.RelationshipEndMembers.All(
                    r => r.RelationshipMultiplicity == RelationshipMultiplicity.Many)
                    ||
                // Many-to-ZeroOrOne 
                relationshipType.RelationshipEndMembers[0].RelationshipMultiplicity == RelationshipMultiplicity.Many
                    && relationshipType.RelationshipEndMembers[1].RelationshipMultiplicity == RelationshipMultiplicity.ZeroOrOne;
        }
    }
}