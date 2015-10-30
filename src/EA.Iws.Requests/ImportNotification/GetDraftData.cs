namespace EA.Iws.Requests.ImportNotification
{
    using System;
    using Prsd.Core.Mediator;

    public class GetDraftData<TData> : IRequest<TData>
    {
        public GetDraftData(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }

        public Guid ImportNotificationId { get; private set; }
    }
}