namespace EA.Iws.RequestHandlers.Annexes
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication.Annexes;
    using Prsd.Core.Mediator;
    using Requests.Annexes;

    internal class DeleteAnnexFileHandler : IRequestHandler<DeleteAnnexFile, bool>
    {
        private readonly IAnnexCollectionRepository annexCollectionRepository;
        private readonly IwsContext context;

        public DeleteAnnexFileHandler(IAnnexCollectionRepository annexCollectionRepository,
            IwsContext context)
        {
            this.annexCollectionRepository = annexCollectionRepository;
            this.context = context;
        }

        public async Task<bool> HandleAsync(DeleteAnnexFile message)
        {
            var annexCollection = await annexCollectionRepository.GetByNotificationId(message.NotificationId);

            annexCollection.RemoveAnnex(message.FileId);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
