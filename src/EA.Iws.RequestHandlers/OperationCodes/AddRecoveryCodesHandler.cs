namespace EA.Iws.RequestHandlers.OperationCodes
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.OperationCodes;

    internal class AddRecoveryCodesHandler : IRequestHandler<AddRecoveryCodes, Guid>
    {
        private readonly IwsContext db;
        private readonly IMap<IList<RecoveryCode>, IList<OperationCode>> mapper;

        public AddRecoveryCodesHandler(IwsContext db, IMap<IList<RecoveryCode>, IList<OperationCode>> mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<Guid> HandleAsync(AddRecoveryCodes command)
        {
            var recoveryCodes = mapper.Map(command.RecoveryCodes);

            var notification = await db.NotificationApplications.SingleAsync(n => n.Id == command.NotificationId);

            notification.UpdateOperationCodes(recoveryCodes);

            await db.SaveChangesAsync();

            return command.NotificationId;
        }
    }
}
