namespace EA.Iws.RequestHandlers.ImportNotification
{
    using System.Threading.Tasks;
    using DataAccess.Draft;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;

    internal class GetDraftDataHandler<TData> : IRequestHandler<GetDraftData<TData>, TData>
    {
        private readonly IDraftImportNotificationRepository repository;

        public GetDraftDataHandler(IDraftImportNotificationRepository repository)
        {
            this.repository = repository;
        }

        public async Task<TData> HandleAsync(GetDraftData<TData> message)
        {
            return await repository.GetDraftData<TData>(message.ImportNotificationId);
        }
    }
}