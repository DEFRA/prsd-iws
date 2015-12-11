namespace EA.Iws.RequestHandlers.ImportMovement.Reject
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportMovement;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement.Reject;

    public class RecordRejectionHandler : IRequestHandler<RecordRejection, bool>
    {
        private readonly IRejectImportMovement rejectImportMovement;
        private readonly ImportNotificationContext context;

        public RecordRejectionHandler(IRejectImportMovement rejectImportMovement, ImportNotificationContext context)
        {
            this.rejectImportMovement = rejectImportMovement;
            this.context = context;
        }

        public async Task<bool> HandleAsync(RecordRejection message)
        {
            var movement = await rejectImportMovement.Reject(message.ImportMovementId, 
                new DateTimeOffset(message.Date, TimeSpan.Zero), 
                message.Reason, 
                message.FurtherDetails);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
