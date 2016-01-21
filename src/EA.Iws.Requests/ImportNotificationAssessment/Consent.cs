namespace EA.Iws.Requests.ImportNotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanEditImportNotificationAssessment)]
    public class Consent : IRequest<bool>
    {
        public Guid ImportNotificationId { get; private set; }

        public DateTime From { get; private set; }

        public DateTime To { get; private set; }

        public string Conditions { get; private set; }

        public DateTime Date { get; private set; }

        public Consent(Guid importNotificationId, DateTime from, DateTime to, string conditions, DateTime date)
        {
            ImportNotificationId = importNotificationId;
            From = from;
            To = to;
            Conditions = conditions;
            Date = date;
        }
    }
}
