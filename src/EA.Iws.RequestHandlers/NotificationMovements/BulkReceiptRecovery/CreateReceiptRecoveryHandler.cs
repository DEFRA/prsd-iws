namespace EA.Iws.RequestHandlers.NotificationMovements.BulkReceiptRecovery
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.FileStore;
    using Domain.Movement;
    using Domain.Movement.BulkUpload;
    using Domain.NotificationApplication;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.BulkUpload;

    public class CreateReceiptRecoveryHandler : IRequestHandler<CreateReceiptRecovery, bool>
    {
        private readonly IDraftMovementRepository draftMovementRepository;
        private readonly IMovementRepository movementRepository;
        private readonly IwsContext context;
        private readonly IUserContext userContext;
        private readonly IFileRepository fileRepository;
        private readonly CertificateFactory certificateFactory;
        private readonly CertificateOfReceiptNameGenerator receiptNameGenerator;
        private readonly CertificateOfRecoveryNameGenerator recoveryNameGenerator;

        public CreateReceiptRecoveryHandler(IDraftMovementRepository draftMovementRepository,
            IMovementRepository movementRepository,
            IwsContext context,
            IUserContext userContext,
            IFileRepository fileRepository,
            CertificateFactory certificateFactory,
            CertificateOfReceiptNameGenerator receiptNameGenerator,
            CertificateOfRecoveryNameGenerator recoveryNameGenerator)
        {
            this.draftMovementRepository = draftMovementRepository;
            this.movementRepository = movementRepository;
            this.context = context;
            this.userContext = userContext;
            this.fileRepository = fileRepository;
            this.certificateFactory = certificateFactory;
            this.receiptNameGenerator = receiptNameGenerator;
            this.recoveryNameGenerator = recoveryNameGenerator;
        }

        public async Task<bool> HandleAsync(CreateReceiptRecovery message)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var draftMovements = await draftMovementRepository.GetDraftMovementById(message.DraftBulkUploadId);

                    if (!draftMovements.Any())
                    {
                        return false;
                    }

                    foreach (var draftMovement in draftMovements)
                    {
                        var movement = await movementRepository.GetByNumberOrDefault(draftMovement.ShipmentNumber,
                            message.NotificationId);

                        if (draftMovement.ReceivedDate.HasValue)
                        {
                            var fileId =
                                await
                                    SaveSupportingDocument(movement, receiptNameGenerator, message.SupportingDocument, message.FileExtension);

                            movement.Receive(fileId, draftMovement.ReceivedDate.GetValueOrDefault(),
                                new Domain.ShipmentQuantity(draftMovement.Quantity, draftMovement.Units),
                                userContext.UserId);

                            await context.SaveChangesAsync();
                        }

                        if (draftMovement.RecoveredDisposedDate.HasValue)
                        {
                            var fileId =
                                await
                                    SaveSupportingDocument(movement, recoveryNameGenerator, message.SupportingDocument,
                                        message.FileExtension);

                            movement.Complete(draftMovement.RecoveredDisposedDate.Value, fileId, userContext.UserId);

                            await context.SaveChangesAsync();
                        }
                    }

                    await draftMovementRepository.DeleteDraftMovementByNotificationId(message.NotificationId);
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                transaction.Commit();
            }

            return true;
        }

        private async Task<Guid> SaveSupportingDocument(Movement movement,
            ICertificateNameGenerator nameGenerator,
            byte[] supportingDocument,
            string fileExtension)
        {
            var receipt =
                await certificateFactory.CreateForMovement(nameGenerator, movement, supportingDocument, fileExtension);

            var fileId = await fileRepository.Store(receipt);

            await context.SaveChangesAsync();

            return fileId;
        }
    }
}
