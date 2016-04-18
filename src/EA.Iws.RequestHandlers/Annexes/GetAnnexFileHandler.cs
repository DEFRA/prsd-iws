namespace EA.Iws.RequestHandlers.Annexes
{
    using System.Threading.Tasks;
    using Core.Annexes;
    using Domain.FileStore;
    using Domain.Security;
    using Prsd.Core.Mediator;
    using Requests.Annexes;

    internal class GetAnnexFileHandler : IRequestHandler<GetAnnexFile, AnnexFileData>
    {
        private readonly IFileRepository fileRepository;
        private readonly INotificationApplicationAuthorization authorization;

        public GetAnnexFileHandler(IFileRepository fileRepository, INotificationApplicationAuthorization authorization)
        {
            this.fileRepository = fileRepository;
            this.authorization = authorization;
        }

        public async Task<AnnexFileData> HandleAsync(GetAnnexFile message)
        {
            await authorization.EnsureAccessAsync(message.NotificationId);

            var file = await fileRepository.Get(message.FileId);

            return new AnnexFileData
            {
                Name = file.Name,
                Type = file.Type,
                Content = file.Content
            };
        }
    }
}
