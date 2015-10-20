namespace EA.Iws.RequestHandlers.Movement
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.FileStore;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class SaveCertificateOfReceiptFileHandler : IRequestHandler<SaveCertificateOfReceiptFile, Guid>
    {
        private readonly IMovementRepository movementRepository;
        private readonly CertificateOfReceiptNameGenerator nameGenerator;
        private readonly CertificateFactory certificateFactory;
        private readonly IwsContext context;
        private readonly IFileRepository fileRepository;

        public SaveCertificateOfReceiptFileHandler(IwsContext context,
            IFileRepository fileRepository,
            IMovementRepository movementRepository,
            CertificateFactory certificateFactory,
            CertificateOfReceiptNameGenerator nameGenerator)
        {
            this.context = context;
            this.fileRepository = fileRepository;
            this.certificateFactory = certificateFactory;
            this.nameGenerator = nameGenerator;
            this.movementRepository = movementRepository;
        }

        public async Task<Guid> HandleAsync(SaveCertificateOfReceiptFile message)
        {
            var movement = await movementRepository.GetById(message.MovementId);

            var receipt = await certificateFactory.CreateForMovement(nameGenerator, movement, message.CertificateBytes, message.FileType);

            var fileId = await fileRepository.Store(receipt);

            await context.SaveChangesAsync();

            return fileId;
        }
    }
}