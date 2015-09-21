namespace EA.Iws.Requests.Movement
{
    using System;
    using Core.MovementOperation;
    using Prsd.Core.Mediator;

    public class GetActiveMovementsWithReceiptCertificateByNotificationId : IRequest<MovementOperationData>
    {
        public Guid Id { get; private set; }

        public GetActiveMovementsWithReceiptCertificateByNotificationId(Guid id)
        {
            Id = id;
        }
    }
}
