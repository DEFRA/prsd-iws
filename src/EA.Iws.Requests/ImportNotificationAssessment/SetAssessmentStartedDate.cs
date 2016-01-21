namespace EA.Iws.Requests.ImportNotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanEditImportNotificationAssessment)]
    public class SetAssessmentStartedDate : IRequest<bool>
    {
        public Guid ImportNotificationId { get; private set; }

        public DateTime Date { get; private set; }

        public string NameOfOfficer { get; private set; }

        public SetAssessmentStartedDate(Guid importNotificationId, DateTime date, string nameOfOfficer)
        {
            ImportNotificationId = importNotificationId;
            Date = date;
            NameOfOfficer = nameOfOfficer;
        }
    }
}
