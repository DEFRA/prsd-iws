namespace EA.Iws.Requests.WasteType
{
    using System;
    using Prsd.Core.Mediator;
    using Security;

    [NotificationReadOnlyAuthorize]
    public class SetWasteGenerationProcess : IRequest<Guid>
    {
        public SetWasteGenerationProcess(string process, Guid notificationId, bool isDocumentAttached)
        {
            Process = process;
            NotificationId = notificationId;
            IsDocumentAttached = isDocumentAttached;
        }
        public string Process { get; private set; }
        public Guid NotificationId { get; private set; }
        public bool IsDocumentAttached { get; private set; }
    }
}