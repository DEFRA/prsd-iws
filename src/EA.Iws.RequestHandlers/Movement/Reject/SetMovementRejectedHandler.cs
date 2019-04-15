namespace EA.Iws.RequestHandlers.Movement.Reject
{
    using System;
    using System.Threading.Tasks;
    using Core.Movement;
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
        private readonly IMovementAuditRepository movementAuditRepository;

        public SetMovementRejectedHandler(IRejectMovement rejectMovement, 
            IMovementRepository movementRepository, 
            IwsContext context,
            MovementFileNameGenerator nameGenerator,
            CertificateFactory certificateFactory,
            IFileRepository fileRepository,
            IMovementAuditRepository movementAuditRepository)
        {
            this.rejectMovement = rejectMovement;
            this.movementRepository = movementRepository;
            this.context = context;
            this.nameGenerator = nameGenerator;
            this.certificateFactory = certificateFactory;
            this.fileRepository = fileRepository;
            this.movementAuditRepository = movementAuditRepository;
        }

        public async Task<Guid> HandleAsync(SetMovementRejected message)
        {
            var movement = await movementRepository.GetById(message.MovementId);

            var file = await certificateFactory.CreateForMovement(nameGenerator, movement, message.FileBytes, message.FileType);
            var fileId = await fileRepository.Store(file);

            var movementRejection = await rejectMovement.Reject(message.MovementId,
                message.DateReceived,
                message.Reason);

            movementRejection.SetFile(fileId);

            await movementAuditRepository.Add(movement, MovementAuditType.Rejected);
            
            await context.SaveChangesAsync();

            return message.MovementId;
        }
    }
}
