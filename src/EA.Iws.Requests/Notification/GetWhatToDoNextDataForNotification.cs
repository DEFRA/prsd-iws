namespace EA.Iws.Requests.Notification
{
    using System;
    using Prsd.Core.Mediator;

    public class GetWhatToDoNextDataForNotification : IRequest<WhatToDoNextData>
    {
        public Guid Id { get; private set; }

        public GetWhatToDoNextDataForNotification(Guid id)
        {
            Id = id;
        }
    }
}
