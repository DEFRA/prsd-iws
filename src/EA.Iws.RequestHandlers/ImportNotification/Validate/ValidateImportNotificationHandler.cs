namespace EA.Iws.RequestHandlers.ImportNotification.Validate
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DataAccess.Draft;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification.Validate;
    using Validation;

    internal class ValidateImportNotificationHandler : IRequestHandler<ValidateImportNotification, IEnumerable<ValidationResults>>
    {
        private readonly IValidator validator;
        private readonly IDraftImportNotificationRepository repository;

        public ValidateImportNotificationHandler(IDraftImportNotificationRepository repository,
            IValidator validator)
        {
            this.repository = repository;
            this.validator = validator;
        }

        public async Task<IEnumerable<ValidationResults>> HandleAsync(ValidateImportNotification message)
        {
            var notificationDraft = await repository.Get(message.DraftImportNotificationId);

            var result = new List<ValidationResults>();

            result.Add(await validator.ValidateAsync(notificationDraft.Preconsented));
            result.Add(await validator.ValidateAsync(notificationDraft.Exporter));
            result.Add(await validator.ValidateAsync(notificationDraft.Importer));
            result.Add(await validator.ValidateAsync(notificationDraft.Producer));
            result.Add(await validator.ValidateAsync(notificationDraft.Facilities));
            result.Add(await validator.ValidateAsync(notificationDraft.Shipment));
            result.Add(await validator.ValidateAsync(notificationDraft.WasteOperation));
            result.Add(await validator.ValidateAsync(notificationDraft.ChemicalComposition));
            result.Add(await validator.ValidateAsync(notificationDraft.WasteType));
            result.Add(await validator.ValidateAsync(notificationDraft.StateOfExport));
            result.Add(await validator.ValidateAsync(notificationDraft.StateOfImport));
            result.Add(await validator.ValidateAsync(notificationDraft.TransitStates));

            return result;
        }
    }
}