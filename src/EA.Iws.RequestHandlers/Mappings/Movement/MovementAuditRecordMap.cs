namespace EA.Iws.RequestHandlers.Mappings.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Movement;
    using Core.Shared;
    using Domain;
    using Domain.Movement;
    using Prsd.Core.Mapper;

    internal class MovementAuditRecordMap : IMap<MovementAudit, ShipmentAuditRecord>
    {
        private readonly IInternalUserRepository internalUserRepository;
        private readonly INotificationUserRepository notificationUserRepository;

        public MovementAuditRecordMap(IInternalUserRepository internalUserRepository, INotificationUserRepository notificationUserRepository)
        {
            this.internalUserRepository = internalUserRepository;
            this.notificationUserRepository = notificationUserRepository;
        }

        public ShipmentAuditRecord Map(MovementAudit audit)
        {
            bool isInternalUser = Task.Run(() => internalUserRepository.IsUserInternal(audit.UserId)).Result;
            string userName = !isInternalUser ? "External User" : Task.Run(() => notificationUserRepository.GetUserByUserId(audit.UserId)).Result.FullName;

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
