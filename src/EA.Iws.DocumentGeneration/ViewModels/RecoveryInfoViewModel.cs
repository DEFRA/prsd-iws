namespace EA.Iws.DocumentGeneration.ViewModels
{
    using Domain.NotificationApplication;
    using Formatters;

    internal class RecoveryInfoViewModel
    {
        private string methodOfDisposal = string.Empty;
        public string MethodOfDisposal
        {
            get { return methodOfDisposal; }
        }

        private string percentageRecoverable = string.Empty;
        public string PercentageRecoverable
        {
            get { return percentageRecoverable; }
        }

        private string estimatedAmountText = string.Empty;
        public string EstimatedAmountText
        {
            get { return estimatedAmountText; }
        }

        private string costAmountText = string.Empty;
        public string CostAmountText
        {
            get { return costAmountText; }
        }

        private string disposalAmountText = string.Empty;
        public string DisposalAmountText
        {
            get { return disposalAmountText; }
        }

        private string annexMessage = string.Empty;
        public string AnnexMessage
        {
            get { return annexMessage; }
        }

        public RecoveryInfoViewModel(NotificationApplication notification,
            RecoveryInfo recoveryInfo,
            RecoveryInfoFormatter recoveryInfoFormatter)
        {
            if (notification == null)
            {
                return;
            }

            if (notification.IsProvidedByImporter.GetValueOrDefault())
            {
                annexMessage = string.Empty;
                return;
            }

            methodOfDisposal = notification.MethodOfDisposal ?? string.Empty;

            percentageRecoverable = recoveryInfoFormatter.NullableDecimalAsPercentage(notification.PercentageRecoverable);

            estimatedAmountText = recoveryInfoFormatter
                .CostAmountWithUnits(recoveryInfo, 
                    ri => ri.EstimatedValue);

            costAmountText = recoveryInfoFormatter
                .CostAmountWithUnits(recoveryInfo,
                    ri => ri.RecoveryCost);

            disposalAmountText = recoveryInfoFormatter
                .CostAmountWithUnits(recoveryInfo,
                    ri => ri.DisposalCost);

            annexMessage = string.Empty;
        }

        public RecoveryInfoViewModel(RecoveryInfoViewModel model, int annexNumber)
        {
            methodOfDisposal = model.MethodOfDisposal;
            percentageRecoverable = model.PercentageRecoverable;
            estimatedAmountText = model.EstimatedAmountText;
            costAmountText = model.CostAmountText;
            disposalAmountText = model.DisposalAmountText;
            annexMessage = "See Annex " + annexNumber;
        }
    }
}
