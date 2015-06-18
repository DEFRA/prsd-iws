namespace EA.Iws.Requests.Notification
{
    using System;
    using Prsd.Core.Mediator;

    public class SetRecoveryPercentageData : IRequest<Guid>
    {
        public SetRecoveryPercentageData(Guid notificationId, bool isProvidedByImporter, string methodOfDisposal, decimal? percentageRecoverable)
        {
            NotificationId = notificationId;
            IsProvidedByImporter = isProvidedByImporter;
            MethodOfDisposal = methodOfDisposal;
            PercentageRecoverable = percentageRecoverable;
        }

        public bool IsProvidedByImporter { get; private set; }
        public Guid NotificationId { get; private set; }
        public string MethodOfDisposal { get; private set; }
        public decimal? PercentageRecoverable { get; private set; }
    }
}
