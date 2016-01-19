namespace EA.Iws.Requests.ImportNotificationAssessment
{
    using System;
    using Prsd.Core.Mediator;

    public class ObjectToImportNotification : IRequest<bool>
    {
        public string ReasonsForObjection { get; private set; }

        public Guid Id { get; private set; }

        public DateTime Date { get; private set; }

        public ObjectToImportNotification(Guid id, string reasonsForObjection, DateTime date)
        {
            ReasonsForObjection = reasonsForObjection;
            Id = id;
            Date = date;
        }
    }
}
