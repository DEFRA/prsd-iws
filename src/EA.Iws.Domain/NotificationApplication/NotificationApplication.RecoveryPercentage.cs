namespace EA.Iws.Domain.NotificationApplication
{
    using System;

    public partial class NotificationApplication
    {
        private decimal? percentageRecoverable;
        public decimal? PercentageRecoverable
        {
            get { return percentageRecoverable; }
            private set
            {
                percentageRecoverable = (value.HasValue) ?
                    decimal.Round(value.Value, 2, MidpointRounding.AwayFromZero)
                    : value;
            }
        }

        public string MethodOfDisposal { get; private set; }
        
        public void SetPercentageRecoverable(decimal percentageRecoverableMaterial)
        {
            PercentageRecoverable = percentageRecoverableMaterial;

            if (PercentageRecoverable > 100)
            {
                throw new InvalidOperationException("The percentage recoverable cannot be greater than 100%");
            }

            if (PercentageRecoverable < 0)
            {
                throw new InvalidOperationException("The percentage recoverable cannot be less than 0%");
            }

            if (PercentageRecoverable == 100)
            {
                MethodOfDisposal = null;
            }

            RecoveryInformationProvidedByImporter = null;
        }

        public void SetMethodOfDisposal(string methodOfDisposal)
        {
            if (PercentageRecoverable == 100 && methodOfDisposal != null)
            {
                throw new InvalidOperationException("When the recovery percentage is 100% there cannot be any method of disposal text");
            }

            RecoveryInformationProvidedByImporter = null;
            MethodOfDisposal = methodOfDisposal;
        }
    }
}