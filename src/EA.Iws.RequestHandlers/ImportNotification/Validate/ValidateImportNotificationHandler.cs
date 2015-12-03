namespace EA.Iws.RequestHandlers.ImportNotification.Validate
{
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess.Draft;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification.Validate;

    internal class ValidateImportNotificationHandler : IRequestHandler<ValidateImportNotification, string[]>
    {
        private readonly IDraftImportNotificationValidator validator;
        private readonly IDraftImportNotificationRepository repository;

        public ValidateImportNotificationHandler(IDraftImportNotificationRepository repository,
            IDraftImportNotificationValidator validator)
        {
            this.repository = repository;
            this.validator = validator;
        }

        public async Task<string[]> HandleAsync(ValidateImportNotification message)
        {
            var notificationDraft = await repository.Get(message.DraftImportNotificationId);

            return validator.Validate(notificationDraft).ToArray();
        }
    }
}