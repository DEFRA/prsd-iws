namespace EA.Iws.DataAccess.Draft
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    internal class DraftImportNotificationRepository : IDraftImportNotificationRepository
    {
        private readonly DraftContext context;

        public DraftImportNotificationRepository(DraftContext context)
        {
            this.context = context;
        }

        public async Task SetDraftData<TData>(Guid importNotificationId, TData data)
        {
            var typeName = typeof(TData).FullName;

            var import = await context.Imports
                .SingleOrDefaultAsync(i => i.ImportNotificationId == importNotificationId && i.Type == typeName);

            var json = JsonConvert.SerializeObject(data);

            if (import == null)
            {
                import = new Import(importNotificationId, typeName, json);
                context.Imports.Add(import);
            }
            else
            {
                import.Value = json;
            }

            await context.SaveChangesAsync();
        }

        public async Task<TData> GetDraftData<TData>(Guid importNotificationId)
        {
            var typeName = typeof(TData).FullName;

            var data = await context.Imports
                .Where(i => i.ImportNotificationId == importNotificationId && i.Type == typeName)
                .Select(p => p.Value)
                .SingleOrDefaultAsync();

            if (data != null)
            {
                return JsonConvert.DeserializeObject<TData>(data);
            }
            else
            {
                return (TData)Activator.CreateInstance(typeof(TData), true);
            }
        }
    }
}