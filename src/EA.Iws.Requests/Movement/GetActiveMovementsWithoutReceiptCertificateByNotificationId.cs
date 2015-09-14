namespace EA.Iws.Requests.Movement
{
    using System;
    using System.Collections.Generic;
    using Core.Movement;
    using Prsd.Core.Mediator;

    public class GetActiveMovementsWithoutReceiptCertificateByNotificationId : IRequest<IList<MovementData>>
    {
        public Guid Id { get; private set; }

        public GetActiveMovementsWithoutReceiptCertificateByNotificationId(Guid id)
        {
            Id = id;
        }
    }
}
