namespace EA.Iws.Requests.ImportNotificationAssessment.Transactions
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanEditImportNotificationAssessment)]
    public class AddNotificationRefund : IRequest<bool>
    {
        public AddNotificationRefund(Guid importNotificationId, decimal amount, DateTime date, string comments)
        {
            Guard.ArgumentNotDefaultValue(() => importNotificationId, importNotificationId);

            ImportNotificationId = importNotificationId;
            Amount = amount;
            Date = date;
            Comments = comments;
        }

        public Guid ImportNotificationId { get; private set; }

        public decimal Amount { get; private set; }

        public DateTime Date { get; private set; }

        public string Comments { get; private set; }
    }
}