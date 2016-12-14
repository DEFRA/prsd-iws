namespace EA.Iws.RequestHandlers.Movement.Complete
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.FileStore;
    using Domain.Movement;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Movement.Complete;

    internal class SaveMovementCompletedReceiptHandler : IRequestHandler<SaveMovementCompletedReceipt, Guid>
    {
        private readonly CertificateOfRecoveryNameGenerator nameGenerator;
        private readonly IUserContext userContext;
        private readonly CertificateFactory certificateFactory;
        private readonly IMovementRepository movementRepository;
        private readonly IFileRepository fileRepository;
        private readonly IwsContext context;

        public SaveMovementCompletedReceiptHandler(IwsContext context,
            IFileRepository fileRepository,
            IMovementRepository movementRepository,
            CertificateFactory certificateFactory,
            CertificateOfRecoveryNameGenerator nameGenerator,
            IUserContext userContext)
        {
            this.context = context;
            this.fileRepository = fileRepository;
            this.movementRepository = movementRepository;
            this.certificateFactory = certificateFactory;
            this.nameGenerator = nameGenerator;
            this.userContext = userContext;
        }

        public async Task<Guid> HandleAsync(SaveMovementCompletedReceipt message)
        {
            var movement = await movementRepository.GetById(message.MovementId);

            var receipt = await certificateFactory.CreateForMovement(nameGenerator, movement, message.CertificateBytes, message.FileType);

            var fileId = await fileRepository.Store(receipt);

            movement.Complete(message.CompletedDate, fileId, userContext.UserId);

            await context.SaveChangesAsync();

            return fileId;
        }
    }
}