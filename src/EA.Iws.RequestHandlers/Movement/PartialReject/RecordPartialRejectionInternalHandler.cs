namespace EA.Iws.RequestHandlers.Movement.PartialReject
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Movement;
    using EA.Iws.Requests.Movement.PartialReject;
    using Prsd.Core.Mediator;

    internal class RecordPartialRejectionInternalHandler : IRequestHandler<RecordPartialRejectionInternal, bool>
    {
        private readonly IPartialRejectionMovement partialRejectionMovement;
        private readonly IwsContext context;

        public RecordPartialRejectionInternalHandler(IPartialRejectionMovement partialRejectionMovement, IwsContext context)
        {
            this.partialRejectionMovement = partialRejectionMovement;
            this.context = context;
        }

        public async Task<bool> HandleAsync(RecordPartialRejectionInternal message)
        {
            await partialRejectionMovement.PartailReject(message.MovementId,
                                                         message.WasteReceivedDate,
                                                         message.Reason,
                                                         message.ActualQuantity,
                                                         message.ActualUnits,
                                                         message.RejectedQuantity,
                                                         message.RejectedUnits,
                                                         message.WasteDisposedDate);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
