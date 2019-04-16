namespace EA.Iws.RequestHandlers.NotificationMovements.BulkPrenotification
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using DataAccess;
    using Domain;
    using Domain.FileStore;
    using Domain.Movement;
    using Domain.Movement.BulkUpload;
    using Domain.NotificationApplication;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.BulkUpload;

    internal class CreatePrenotificationHandler : IRequestHandler<CreateBulkPrenotification, bool>
    {
        private readonly INotificationApplicationRepository notificationRepository;
        private readonly IDraftMovementRepository draftMovementRepository;
        private readonly IwsContext context;
        private readonly MovementFactory movementFactory;
        private readonly MovementDetailsFactory movementDetailsFactory;
        private readonly IFileRepository fileRepository;
        private readonly IMovementAuditRepository movementAuditRepository;
        private readonly IUserContext userContext;

        public CreatePrenotificationHandler(INotificationApplicationRepository notificationRepository,
            IDraftMovementRepository draftMovementRepository,
            IwsContext context,
            MovementFactory movementFactory,
            MovementDetailsFactory movementDetailsFactory,
            IFileRepository fileRepository,
            IMovementAuditRepository movementAuditRepository,
            IUserContext userContext)
        {
            this.notificationRepository = notificationRepository;
            this.draftMovementRepository = draftMovementRepository;
            this.context = context;
            this.movementFactory = movementFactory;
            this.movementDetailsFactory = movementDetailsFactory;
            this.fileRepository = fileRepository;
            this.movementAuditRepository = movementAuditRepository;
            this.userContext = userContext;
        }

        public async Task<bool> HandleAsync(CreateBulkPrenotification message)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var draftMovements = await draftMovementRepository.GetDraftMovementById(message.DraftBulkUploadId);
                    var notification = await notificationRepository.GetById(message.NotificationId);

                    foreach (var draftMovement in draftMovements)
                    {
                        var movement = movementFactory.Create(message.NotificationId, draftMovement.ShipmentNumber,
                            draftMovement.Date.Value);

                        context.Movements.Add(movement);

                        await context.SaveChangesAsync();

                        await SaveMovementDetails(movement, draftMovement, notification);

                        await
                            SaveSupportingDocument(movement, draftMovement, message.FileExtension,
                                message.SupportingDocument);

                        await SavePrenotifiedAudit(movement);
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

        private async Task SaveMovementDetails(Movement movement, DraftMovement draftMovement, NotificationApplication notification)
        {
            var shipmentQuantity = new ShipmentQuantity(draftMovement.Quantity, draftMovement.Units);
            var packagingInfos = GetPackagingInfoFromDraft(draftMovement.PackagingInfos, notification);

            var movementDetails = await movementDetailsFactory.Create(
                movement,
                shipmentQuantity,
                packagingInfos);

            context.MovementDetails.Add(movementDetails);

            await context.SaveChangesAsync();
        }

        private async Task SaveSupportingDocument(Movement movement, 
            DraftMovement draftMovement, 
            string fileExtension, 
            byte[] supportingDocument)
        {
            var fileName = GetFileName(draftMovement.NotificationNumber);
            var file = new File(fileName, fileExtension, supportingDocument);
            var fileId = await fileRepository.Store(file);

            movement.Submit(fileId);

            await context.SaveChangesAsync();
        }

        private async Task SavePrenotifiedAudit(Movement movement)
        {
            await movementAuditRepository.Add(new MovementAudit(movement.NotificationId, movement.Number,
                userContext.UserId.ToString(), (int)MovementAuditType.Prenotified, SystemTime.UtcNow));

            await context.SaveChangesAsync();
        }

        private static IEnumerable<PackagingInfo> GetPackagingInfoFromDraft(IEnumerable<DraftPackagingInfo> draftPackagingInfos,
            NotificationApplication notification)
        {
            var packagingTypes = draftPackagingInfos.Select(p => p.PackagingType);

            return notification.PackagingInfos
                .Where(p => packagingTypes.Contains(p.PackagingType));
        }

        private static string GetFileName(string notificationNumber)
        {
            return string.Format("{0}-shipment-bulk-{1:yyyy-MM-dd-HH-mm-ss}",
                notificationNumber.Replace(" ", string.Empty), SystemTime.UtcNow);
        }
    }
}
