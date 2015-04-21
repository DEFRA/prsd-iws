namespace EA.Iws.Core.DataAccess.Extensions
{
    using System.Data.Entity;
    using System.Linq;
    using Domain;
    using Utils.Identity;

    public static class EntityIdExtensions
    {
        public static void SetEntityId(this DbContext context)
        {
            foreach (var entry in context.ChangeTracker.Entries<Entity>()
                .Where(e => e.State == EntityState.Added))
            {
                typeof(Entity).GetProperty("Id").SetValue(entry.Entity, GuidCombGenerator.GenerateComb());
            }
        }
    }
}