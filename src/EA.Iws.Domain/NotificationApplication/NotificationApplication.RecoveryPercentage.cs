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
        
        public bool? IsProvidedByImporter { get; private set; }
        public string MethodOfDisposal { get; private set; }

        public void SetRecoveryPercentageDataProvidedByImporter()
        {
            IsProvidedByImporter = true;
            PercentageRecoverable = null;
            MethodOfDisposal = null;
        }

        public void SetRecoveryPercentageData(decimal percentageRecoverableMaterial, string methodOfDisposal)
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

            if (PercentageRecoverable < 100 && methodOfDisposal == null)
            {
                throw new InvalidOperationException("If the information is not being provided by the importer then the method of disposal is required");
            }

            if (PercentageRecoverable == 100 && methodOfDisposal != null)
            {
                throw new InvalidOperationException("When the recovery percentage is 100% there cannot be any method of disposal text");
            }

            IsProvidedByImporter = null;
            MethodOfDisposal = methodOfDisposal;
        }
    }
}