namespace EA.Iws.Requests.RecoveryInfo
{
    using System;
    using Prsd.Core.Mediator;

    public class SetRecoveryPercentageData : IRequest<Guid>
    {
        public SetRecoveryPercentageData(Guid notificationId, bool isProvidedByImporter, decimal? percentageRecoverable, string methodOfDisposal)
        {
            NotificationId = notificationId;
            IsProvidedByImporter = isProvidedByImporter;
            PercentageRecoverable = percentageRecoverable;
            MethodOfDisposal = methodOfDisposal;
        }

        public bool IsProvidedByImporter { get; private set; }
        public Guid NotificationId { get; private set; }
        public decimal? PercentageRecoverable { get; private set; }
        public string MethodOfDisposal { get; private set; }
    }
}
