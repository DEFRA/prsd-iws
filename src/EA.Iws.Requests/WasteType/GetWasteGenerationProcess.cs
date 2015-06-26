namespace EA.Iws.Requests.WasteType
{
    using System;
    using Core.WasteType;
    using Prsd.Core.Mediator;

    public class GetWasteGenerationProcess : IRequest<WasteGenerationProcessData>
    {
        public GetWasteGenerationProcess(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}