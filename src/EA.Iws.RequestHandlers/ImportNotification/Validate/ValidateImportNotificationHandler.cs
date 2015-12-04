namespace EA.Iws.RequestHandlers.ImportNotification.Validate
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ImportNotification.Draft;
    using DataAccess.Draft;
    using FluentValidation;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification.Validate;

    internal class ValidateImportNotificationHandler : IRequestHandler<ValidateImportNotification, string[]>
    {
        private readonly IValidator<ImportNotification> validator;
        private readonly IDraftImportNotificationRepository repository;

        public ValidateImportNotificationHandler(IDraftImportNotificationRepository repository,
            IValidator<ImportNotification> validator)
        {
            this.repository = repository;
            this.validator = validator;
        }

        public async Task<string[]> HandleAsync(ValidateImportNotification message)
        {
            var notificationDraft = await repository.Get(message.DraftImportNotificationId);

            var result = await validator.ValidateAsync(notificationDraft);

            return result.Errors.Select(e => e.ErrorMessage).ToArray();
        }
    }
}