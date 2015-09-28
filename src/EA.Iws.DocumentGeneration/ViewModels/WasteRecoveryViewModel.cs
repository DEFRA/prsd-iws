namespace EA.Iws.DocumentGeneration.ViewModels
{
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.WasteRecovery;
    using Formatters;

    internal class WasteRecoveryViewModel
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

        public WasteRecoveryViewModel(NotificationApplication notification,
            WasteRecovery wasteRecovery,
            WasteRecoveryFormatter wasteRecoveryFormatter)
        {
            if (notification == null)
            {
                return;
            }

            if (notification.WasteRecoveryInformationProvidedByImporter.GetValueOrDefault())
            {
                annexMessage = string.Empty;
                return;
            }

            //methodOfDisposal = notification.MethodOfDisposal ?? string.Empty;

            //percentageRecoverable = wasteRecoveryFormatter.NullableDecimalAsPercentage(notification.PercentageRecoverable);

            estimatedAmountText = wasteRecoveryFormatter
                .CostAmountWithUnits(wasteRecovery, 
                    ri => ri.EstimatedValue);

            costAmountText = wasteRecoveryFormatter
                .CostAmountWithUnits(wasteRecovery,
                    ri => ri.RecoveryCost);

            //disposalAmountText = wasteRecoveryFormatter
            //    .CostAmountWithUnits(wasteRecovery,
            //        ri => ri.DisposalCost);

            annexMessage = string.Empty;
        }

        public WasteRecoveryViewModel(WasteRecoveryViewModel model, int annexNumber)
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
