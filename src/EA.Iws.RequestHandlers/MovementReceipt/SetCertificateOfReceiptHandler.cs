namespace EA.Iws.RequestHandlers.MovementReceipt
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.FileStore;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.MovementReceipt;

    internal class SetCertificateOfReceiptHandler : IRequestHandler<SetCertificateOfReceipt, Guid>
    {
        private readonly CertificateOfReceiptFactory certificateOfReceiptFactory;
        private readonly IwsContext context;
        private readonly IFileRepository fileRepository;

        public SetCertificateOfReceiptHandler(IwsContext context,
            IFileRepository fileRepository,
            CertificateOfReceiptFactory certificateOfReceiptFactory)
        {
            this.context = context;
            this.fileRepository = fileRepository;
            this.certificateOfReceiptFactory = certificateOfReceiptFactory;
        }

        public async Task<Guid> HandleAsync(SetCertificateOfReceipt message)
        {
            var movement = await context.Movements.SingleAsync(m => m.Id == message.MovementId);

            var receipt = await certificateOfReceiptFactory.CreateForMovement(movement, message.CertificateBytes, message.FileType);

            var fileId = await fileRepository.Store(receipt);

            movement.Receipt.SetCertificateFile(fileId);

            await context.SaveChangesAsync();

            return fileId;
        }
    }
}