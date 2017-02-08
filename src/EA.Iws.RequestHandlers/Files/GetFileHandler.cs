namespace EA.Iws.RequestHandlers.Files
{
    using System.Threading.Tasks;
    using Core.Files;
    using Domain.FileStore;
    using Domain.Security;
    using Prsd.Core.Mediator;
    using Requests.Files;

    internal class GetFileHandler : IRequestHandler<GetFile, FileData>
    {
        private readonly IFileRepository fileRepository;
        private readonly INotificationApplicationAuthorization authorization;

        public GetFileHandler(IFileRepository fileRepository, INotificationApplicationAuthorization authorization)
        {
            this.fileRepository = fileRepository;
            this.authorization = authorization;
        }

        public async Task<FileData> HandleAsync(GetFile message)
        {
            await authorization.EnsureAccessAsync(message.NotificationId);

            var file = await fileRepository.Get(message.FileId);

            return new FileData
            {
                Name = file.Name,
                Type = file.Type,
                Content = file.Content
            };
        }
    }
}
