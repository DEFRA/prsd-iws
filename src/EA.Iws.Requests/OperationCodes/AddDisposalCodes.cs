namespace EA.Iws.Requests.OperationCodes
{
    using System;
    using System.Collections.Generic;
    using Core.OperationCodes;
    using Prsd.Core.Mediator;

    public class AddDisposalCodes : IRequest<Guid>
    {
        public AddDisposalCodes(List<DisposalCode> disposalCodes, Guid notificationId)
        {
            DisposalCodes = disposalCodes;
            NotificationId = notificationId;
        }

        public List<DisposalCode> DisposalCodes { get; private set; }

        public Guid NotificationId { get; private set; }
    }
}
