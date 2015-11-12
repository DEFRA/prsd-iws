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

        private string percentageNonRecoverable = string.Empty;

        public string PercentageNonRecoverable
        {
            get { return percentageNonRecoverable; }
        }

        public WasteRecoveryViewModel(NotificationApplication notification,
            WasteRecovery wasteRecovery,
            WasteDisposal wasteDisposal,
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

            if (wasteDisposal != null)
            {
                methodOfDisposal = wasteDisposal.Method ?? string.Empty;
            }
            
            if (wasteRecovery != null && wasteRecovery.PercentageRecoverable != null)
            {
                percentageRecoverable = wasteRecoveryFormatter.NullableDecimalAsPercentage(wasteRecovery.PercentageRecoverable.Value);
                percentageNonRecoverable =
                    wasteRecoveryFormatter.NullableDecimalAsPercentage(100 - wasteRecovery.PercentageRecoverable.Value);
            }

            estimatedAmountText = wasteRecoveryFormatter
                .CostAmountWithUnits(wasteRecovery, 
                    ri => ri.EstimatedValue);

            costAmountText = wasteRecoveryFormatter
                .CostAmountWithUnits(wasteRecovery,
                    ri => ri.RecoveryCost);

            disposalAmountText = wasteRecoveryFormatter
                .CostAmountWithUnits(wasteDisposal,
                    ri => ri.Cost);

            annexMessage = string.Empty;
        }

        public WasteRecoveryViewModel(WasteRecoveryViewModel model, int annexNumber)
        {
            methodOfDisposal = model.MethodOfDisposal;
            percentageRecoverable = model.PercentageRecoverable;
            percentageNonRecoverable = model.PercentageNonRecoverable;
            estimatedAmountText = model.EstimatedAmountText;
            costAmountText = model.CostAmountText;
            disposalAmountText = model.DisposalAmountText;
            annexMessage = "See Annex " + annexNumber;
        }
    }
}
