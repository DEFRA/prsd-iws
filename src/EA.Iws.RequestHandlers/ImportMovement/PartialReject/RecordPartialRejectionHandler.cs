namespace EA.Iws.RequestHandlers.ImportMovement.PartialReject
{
    using System.Threading.Tasks;
    using DataAccess;
    using EA.Iws.Domain.ImportMovement;
    using EA.Iws.Requests.ImportMovement.PartialReject;
    using Prsd.Core.Mediator;

    internal class RecordPartialRejectionHandler : IRequestHandler<RecordPartialRejection, bool>
    {
        private readonly IPartialRejectionImportMovement partialRejectionImportMovement;
        private readonly ImportNotificationContext context;

        public RecordPartialRejectionHandler(IPartialRejectionImportMovement partialRejectionImportMovement, ImportNotificationContext context)
        {
            this.partialRejectionImportMovement = partialRejectionImportMovement;
            this.context = context;
        }

        public async Task<bool> HandleAsync(RecordPartialRejection message)
        {
            await partialRejectionImportMovement.PartailReject(message.MovementId,
                                                         message.Date,
                                                         message.Reason,
                                                         message.ActualQuantity,
                                                         message.ActualUnits,
                                                         message.RejectedQuantity,
                                                         message.RejectedUnits);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
