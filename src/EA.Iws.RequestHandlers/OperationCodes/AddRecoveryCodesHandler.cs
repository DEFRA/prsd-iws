namespace EA.Iws.RequestHandlers.OperationCodes
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.OperationCodes;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.OperationCodes;

    internal class AddRecoveryCodesHandler : IRequestHandler<AddRecoveryCodes, Guid>
    {
        private readonly IwsContext context;
        private readonly IMap<IList<RecoveryCode>, IList<OperationCode>> mapper;

        public AddRecoveryCodesHandler(IwsContext context, IMap<IList<RecoveryCode>, IList<OperationCode>> mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Guid> HandleAsync(AddRecoveryCodes command)
        {
            var recoveryCodes = mapper.Map(command.RecoveryCodes);

            var notification = await context.GetNotificationApplication(command.NotificationId);

            notification.SetOperationCodes(recoveryCodes);

            await context.SaveChangesAsync();

            return command.NotificationId;
        }
    }
}