namespace EA.Iws.Requests.ImportNotificationAssessment
{
    using System;
    using Prsd.Core.Mediator;

    public class SetAcknowlegedDate : IRequest<bool>
    {
        public Guid ImportNotificationId { get; private set; }

        public DateTime Date { get; private set; }

        public SetAcknowlegedDate(Guid importNotificationId, DateTime date)
        {
            ImportNotificationId = importNotificationId;
            Date = date;
        }
    }
}
