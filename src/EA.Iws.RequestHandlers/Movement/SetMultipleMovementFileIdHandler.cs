namespace EA.Iws.RequestHandlers.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using DataAccess;
    using Domain.FileStore;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    
    internal class SetMultipleMovementFileIdHandler : IRequestHandler<SetMultipleMovementFileId, Guid>
    {
        private readonly IwsContext context;
        private readonly IMovementRepository movementRepository;
        private readonly INotificationApplicationRepository notificationRepository;
        private readonly IFileRepository fileRepository;
        private readonly IMovementAuditRepository movementAuditRepository;
        private readonly IUserContext userContext;

        public SetMultipleMovementFileIdHandler(IwsContext context,
            IMovementRepository movementRepository,
            INotificationApplicationRepository notificationRepository,
            IFileRepository fileRepository,
            IMovementAuditRepository movementAuditRepository,
            IUserContext userContext)
        {
            this.context = context;
            this.movementRepository = movementRepository;
            this.notificationRepository = notificationRepository;
            this.fileRepository = fileRepository;
            this.movementAuditRepository = movementAuditRepository;
            this.userContext = userContext;
        }

        public async Task<Guid> HandleAsync(SetMultipleMovementFileId message)
        {
            var movements = new List<Movement>();
            foreach (var movementId in message.MovementIds)
            {
                var movement = await movementRepository.GetById(movementId);
                movements.Add(movement);
            }

            var notification = await notificationRepository.GetById(message.NotificationId);
            var fileName = GetFileName(notification.NotificationNumber, movements.Select(m => m.Number));

            var file = new File(fileName, message.FileType, message.MovementBytes);
            var fileId = await fileRepository.Store(file);

            foreach (var movement in movements)
            {
                movement.Submit(fileId);
            }

            await context.SaveChangesAsync();

            foreach (var movement in movements)
            {
                await movementAuditRepository.Add(new MovementAudit(movement.NotificationId, movement.Number,
                    userContext.UserId.ToString(), (int)MovementAuditType.Prenotified, SystemTime.Now));
            }

            await context.SaveChangesAsync();

            return fileId;
        }

        private static string GetFileName(string notificationNumber, IEnumerable<int> shipmentNumbers)
        {
            return string.Format("{0}-shipment-{1}", notificationNumber.Replace(" ", string.Empty),
                string.Join("-", shipmentNumbers));
        }
    }
}
