namespace EA.Iws.Domain.Notification
{
    using System;

    public partial class NotificationApplication
    {
        public bool? IsProvidedByImporter { get; private set; }
        public decimal? PercentageRecoverable { get; private set; }
        public string MethodOfDisposal { get; private set; }

        public void SetRecoveryPercentageDataProvidedByImporter()
        {
            IsProvidedByImporter = true;
            PercentageRecoverable = null;
            MethodOfDisposal = null;
        }

        public void SetRecoveryPercentageData(decimal percentageRecoverable, string methodOfDisposal)
        {
            if (percentageRecoverable > 100)
            {
                throw new InvalidOperationException("The percentage recoverable cannot be greater than 100%");
            }

            if (percentageRecoverable < 0)
            {
                throw new InvalidOperationException("The percentage recoverable cannot be less than 0%");
            }

            if (percentageRecoverable < 100 && methodOfDisposal == null)
            {
                throw new InvalidOperationException("If the information is not being provided by the importer then the method of disposal is required");
            }

            if (percentageRecoverable == 100 && methodOfDisposal != null)
            {
                throw new InvalidOperationException("When the recovery percentage is 100% there cannot be any method of disposal text");
            }

            IsProvidedByImporter = null;
            PercentageRecoverable = percentageRecoverable;
            MethodOfDisposal = methodOfDisposal;
        }
    }
}