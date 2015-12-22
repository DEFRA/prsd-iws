namespace EA.Iws.Requests.Notification
{
    using System;
    using Prsd.Core.Mediator;

    public class GetWhatToDoNextPaymentDataForNotification : IRequest<WhatToDoNextPaymentData>
    {
        public Guid Id { get; private set; }

        public GetWhatToDoNextPaymentDataForNotification(Guid id)
        {
            Id = id;
        }
    }
}
