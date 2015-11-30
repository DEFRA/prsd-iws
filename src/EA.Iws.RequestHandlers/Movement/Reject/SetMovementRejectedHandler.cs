namespace EA.Iws.RequestHandlers.Movement.Reject
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.FileStore;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.Movement.Reject;

    internal class SetMovementRejectedHandler : IRequestHandler<SetMovementRejected, Guid>
    {
        private readonly IRejectMovement rejectMovement;
        private readonly IMovementRepository movementRepository;
        private readonly IwsContext context;
        private readonly ICertificateNameGenerator nameGenerator;
        private readonly CertificateFactory certificateFactory;
        private readonly IFileRepository fileRepository;

        public SetMovementRejectedHandler(IRejectMovement rejectMovement, 
            IMovementRepository movementRepository, 
            IwsContext context,
            MovementFileNameGenerator nameGenerator,
            CertificateFactory certificateFactory,
            IFileRepository fileRepository)
        {
            this.rejectMovement = rejectMovement;
            this.movementRepository = movementRepository;
            this.context = context;
            this.nameGenerator = nameGenerator;
            this.certificateFactory = certificateFactory;
            this.fileRepository = fileRepository;
        }

        public async Task<Guid> HandleAsync(SetMovementRejected message)
        {
            var movement = await movementRepository.GetById(message.MovementId);

            var file = await certificateFactory.CreateForMovement(nameGenerator, movement, message.FileBytes, message.FileType);
            var fileId = await fileRepository.Store(file);

            var movementRejection = await rejectMovement.Reject(message.MovementId,
                new DateTimeOffset(message.DateReceived, TimeSpan.Zero),
                message.Reason);

            movementRejection.SetFile(fileId);
            
            await context.SaveChangesAsync();

            return message.MovementId;
        }
    }
}
