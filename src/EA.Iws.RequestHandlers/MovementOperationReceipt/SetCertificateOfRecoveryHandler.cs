namespace EA.Iws.RequestHandlers.MovementOperationReceipt
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.FileStore;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.MovementOperationReceipt;

    internal class SetCertificateOfRecoveryHandler : IRequestHandler<SetCertificateOfRecovery, Guid>
    {
        private readonly CertificateOfRecoveryNameGenerator nameGenerator;
        private readonly CertificateFactory certificateFactory;
        private readonly IMovementRepository movementRepository;
        private readonly IFileRepository fileRepository;
        private readonly IwsContext context;

        public SetCertificateOfRecoveryHandler(IwsContext context,
            IFileRepository fileRepository,
            IMovementRepository movementRepository,
            CertificateFactory certificateFactory,
            CertificateOfRecoveryNameGenerator nameGenerator)
        {
            this.context = context;
            this.fileRepository = fileRepository;
            this.movementRepository = movementRepository;
            this.certificateFactory = certificateFactory;
            this.nameGenerator = nameGenerator;
        }

        public async Task<Guid> HandleAsync(SetCertificateOfRecovery message)
        {
            var movement = await movementRepository.GetById(message.MovementId);

            var receipt = await certificateFactory.CreateForMovement(nameGenerator, movement, message.CertificateBytes, message.FileType);

            var fileId = await fileRepository.Store(receipt);

            movement.Receipt.OperationReceipt.SetCertificateFile(fileId);

            await context.SaveChangesAsync();

            return fileId;
        }
    }
}