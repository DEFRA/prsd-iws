namespace EA.Iws.DocumentGeneration.ViewModels
{
    using Domain.NotificationApplication;

    internal class RecoveryInfoViewModel
    {
        public RecoveryInfoViewModel(RecoveryInfo recoveryInfo, NotificationApplication notification)
        {
            MethodOfDisposal = notification.MethodOfDisposal;
            PercentageRecoverable = string.Format("{0} %", notification.PercentageRecoverable.GetValueOrDefault());
            EstimatedAmountText = string.Format("£ {0} / {1}", recoveryInfo.EstimatedAmount, recoveryInfo.EstimatedUnit);
            CostAmountText = string.Format("£ {0} / {1}", recoveryInfo.CostAmount, recoveryInfo.CostUnit);
            DisposalAmountText = recoveryInfo.DisposalAmount.HasValue ?
                             string.Format("£ {0} / {1}", recoveryInfo.DisposalAmount, recoveryInfo.DisposalUnit) :
                             string.Empty;
            AnnexMessage = string.Empty;
        }

        public RecoveryInfoViewModel(RecoveryInfoViewModel model, int annexNumber)
        {
            MethodOfDisposal = model.MethodOfDisposal;
            PercentageRecoverable = model.PercentageRecoverable;
            EstimatedAmountText = model.EstimatedAmountText;
            CostAmountText = model.CostAmountText;
            DisposalAmountText = model.DisposalAmountText;
            AnnexMessage = "See Annex " + annexNumber;
        }

        public string MethodOfDisposal { get; private set; }
        public string PercentageRecoverable { get; private set; }
        public string EstimatedAmountText { get; private set; }
        public string CostAmountText { get; private set; }
        public string DisposalAmountText { get; private set; }
        public string AnnexMessage { get; private set; }
    }
}
