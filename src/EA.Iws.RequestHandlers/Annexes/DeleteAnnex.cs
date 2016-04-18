namespace EA.Iws.RequestHandlers.Annexes
{
    using System.Threading.Tasks;
    using DataAccess.Filestore;
    using Domain.FileStore;
    using Domain.NotificationApplication.Annexes;
    using Prsd.Core.Domain;

    internal class DeleteAnnex : IEventHandler<DeleteAnnexEvent>
    {
        private readonly IFileRepository fileRepository;
        private readonly IwsFileStoreContext fileContext;

        public DeleteAnnex(IFileRepository fileRepository, IwsFileStoreContext fileContext)
        {
            this.fileRepository = fileRepository;
            this.fileContext = fileContext;
        }

        public async Task HandleAsync(DeleteAnnexEvent deleteAnnexEvent)
        {
            await fileRepository.Remove(deleteAnnexEvent.FileId);

            await fileContext.SaveChangesAsync();
        }
    }
}
