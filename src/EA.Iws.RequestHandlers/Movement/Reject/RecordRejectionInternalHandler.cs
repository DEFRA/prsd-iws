namespace EA.Iws.RequestHandlers.Movement.Reject
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.Movement.Reject;

    internal class RecordRejectionInternalHandler : IRequestHandler<RecordRejectionInternal, bool>
    {
        private readonly IRejectMovement rejectMovement;
        private readonly IwsContext context;

        public RecordRejectionInternalHandler(IRejectMovement rejectMovement, IwsContext context)
        {
            this.rejectMovement = rejectMovement;
            this.context = context;
        }

        public async Task<bool> HandleAsync(RecordRejectionInternal message)
        {
            await rejectMovement.Reject(message.MovementId,
                message.RejectedDate,
                message.RejectionReason,
                message.RejectionFurtherInformation);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
