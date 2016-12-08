namespace EA.Iws.DataAccess.Draft
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ImportNotification.Draft;
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

            return DeserializeDraftObject<TData>(importNotificationId, data);
        }

        public async Task<ImportNotification> Get(Guid importNotificationId)
        {
            var data = await context.Imports
                .Where(i => i.ImportNotificationId == importNotificationId)
                .ToListAsync();

            return new ImportNotification
            {
                Exporter = GetDraftData<Exporter>(data, importNotificationId),
                Importer = GetDraftData<Importer>(data, importNotificationId),
                Facilities = GetDraftData<FacilityCollection>(data, importNotificationId),
                Preconsented = GetDraftData<Preconsented>(data, importNotificationId),
                Producer = GetDraftData<Producer>(data, importNotificationId),
                Shipment = GetDraftData<Shipment>(data, importNotificationId),
                StateOfExport = GetDraftData<StateOfExport>(data, importNotificationId),
                StateOfImport = GetDraftData<StateOfImport>(data, importNotificationId),
                TransitStates = GetDraftData<TransitStateCollection>(data, importNotificationId),
                WasteOperation = GetDraftData<WasteOperation>(data, importNotificationId),
                WasteType = GetDraftData<WasteType>(data, importNotificationId),
                ChemicalComposition = GetDraftData<ChemicalComposition>(data, importNotificationId)
            };
        }

        private static TData DeserializeDraftObject<TData>(Guid importNotificationId, string json)
        {
            if (json != null)
            {
                return JsonConvert.DeserializeObject<TData>(json);
            }
            else if (typeof(IDraftEntity).IsAssignableFrom(typeof(TData)))
            {
                return (TData)Activator.CreateInstance(typeof(TData), importNotificationId);
            }
            else
            {
                return (TData)Activator.CreateInstance(typeof(TData), true);
            }
        }

        private static TData GetDraftData<TData>(IEnumerable<Import> importData, Guid importNotificationId)
        {
            var typeName = typeof(TData).FullName;

            var data = importData.SingleOrDefault(x => x.Type == typeName);

            return DeserializeDraftObject<TData>(importNotificationId, data == null ? null : data.Value);
        }
    }
}