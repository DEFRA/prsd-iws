namespace EA.Iws.Requests.CustomsOffice
{
    using System;
    using Prsd.Core.Mediator;

    public class GetCustomsCompletionStatusByNotificationId : IRequest<CustomsOfficeCompletionStatus>
    {
        public Guid Id { get; private set; }

        public GetCustomsCompletionStatusByNotificationId(Guid id)
        {
            Id = id;
        }
    }
}
