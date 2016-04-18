namespace EA.Iws.RequestHandlers.ImportNotification
{
    using System.Threading.Tasks;
    using DataAccess.Draft;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;

    internal class SetDraftDataHandler<TData> : IRequestHandler<SetDraftData<TData>, bool>
    {
        private readonly IDraftImportNotificationRepository repository;

        public SetDraftDataHandler(IDraftImportNotificationRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> HandleAsync(SetDraftData<TData> message)
        {
            await repository.SetDraftData(message.ImportNotificationId, message.Data);

            return true;
        }
    }
}