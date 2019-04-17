namespace EA.Iws.RequestHandlers.Mappings.ImportMovement
{
    using System.Threading.Tasks;
    using Core.Movement;
    using Core.Shared;
    using Domain;
    using Domain.ImportMovement;
    using Prsd.Core.Mapper;

    internal class MovementAuditRecordMap : IMap<ImportMovementAudit, ShipmentAuditRecord>
    {
        private readonly INotificationUserRepository notificationUserRepository;

        public MovementAuditRecordMap(INotificationUserRepository notificationUserRepository)
        {
            this.notificationUserRepository = notificationUserRepository;
        }

        public ShipmentAuditRecord Map(ImportMovementAudit audit)
        {
            string userName = Task.Run(() => notificationUserRepository.GetUserByUserId(audit.UserId)).Result.FullName;

            return new ShipmentAuditRecord()
            {
                ShipmentNumber = audit.ShipmentNumber,
                AuditType = ((MovementAuditType)audit.Type),
                DateAdded = audit.DateAdded,
                UserName = userName
            };
        }
    }
}
