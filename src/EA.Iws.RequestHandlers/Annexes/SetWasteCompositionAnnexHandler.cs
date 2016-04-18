namespace EA.Iws.RequestHandlers.Annexes
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.FileStore;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Annexes;
    using Prsd.Core.Mediator;
    using Requests.Annexes;

    internal class SetWasteCompositionAnnexHandler : IRequestHandler<SetWasteCompositionAnnex, bool>
    {
        private readonly IwsContext context;
        private readonly IAnnexCollectionRepository annexCollectionRepository;
        private readonly IFileRepository fileRepository;
        private readonly AnnexFactory annexFactory;
        private readonly WasteCompositionNameGenerator nameGenerator;
        private readonly INotificationApplicationRepository notificationApplicationRepository;

        public SetWasteCompositionAnnexHandler(IwsContext context, 
            IAnnexCollectionRepository annexCollectionRepository, 
            IFileRepository fileRepository,
            AnnexFactory annexFactory,
            WasteCompositionNameGenerator nameGenerator,
            INotificationApplicationRepository notificationApplicationRepository)
        {
            this.context = context;
            this.annexCollectionRepository = annexCollectionRepository;
            this.fileRepository = fileRepository;
            this.annexFactory = annexFactory;
            this.nameGenerator = nameGenerator;
            this.notificationApplicationRepository = notificationApplicationRepository;
        }

        public async Task<bool> HandleAsync(SetWasteCompositionAnnex message)
        {
            var annexCollection = await annexCollectionRepository.GetByNotificationId(message.Annex.NotificationId);

            var file = annexFactory.CreateForNotification(nameGenerator, 
                await notificationApplicationRepository.GetById(message.Annex.NotificationId),
                message.Annex.FileBytes,
                message.Annex.FileType);

            var fileId = await fileRepository.Store(file);

            annexCollection.SetWasteCompositionAnnex(new WasteCompositionAnnex(fileId));

            await context.SaveChangesAsync();

            return true;
        }
    }
}
