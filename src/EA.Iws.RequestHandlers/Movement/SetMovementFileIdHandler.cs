namespace EA.Iws.RequestHandlers.Movement
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.FileStore;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    
    internal class SetMovementFileIdHandler : IRequestHandler<SetMovementFileId, Guid>
    {
        private readonly IwsContext context;
        private readonly IMovementRepository movementRepository;
        private readonly MovementFileNameGenerator nameGenerator;
        private readonly CertificateFactory certificateFactory;
        private readonly IFileRepository fileRepository;

        public SetMovementFileIdHandler(IwsContext context,
            IMovementRepository movementRepository,
            MovementFileNameGenerator nameGenerator,
            CertificateFactory certificateFactory,
            IFileRepository fileRepository)
        {
            this.context = context;
            this.movementRepository = movementRepository;
            this.nameGenerator = nameGenerator;
            this.certificateFactory = certificateFactory;
            this.fileRepository = fileRepository;
        }

        public async Task<Guid> HandleAsync(SetMovementFileId message)
        {
            var movement = await movementRepository.GetById(message.MovementId);

            await context.SaveChangesAsync();
            var file = await certificateFactory.CreateForMovement(nameGenerator, movement, message.MovementBytes, message.FileType);
            var fileId = await fileRepository.Store(file);

            movement.Submit(fileId);

            await context.SaveChangesAsync();

            return fileId;
        }
    }
}
