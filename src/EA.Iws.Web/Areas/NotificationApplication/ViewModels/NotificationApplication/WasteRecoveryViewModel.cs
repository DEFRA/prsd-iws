namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.NotificationApplication
{
    using System;
    using Core.Notification;
    using Core.Notification.Overview;
    using Core.Shared;

    public class WasteRecoveryViewModel
    {
        public Guid NotificationId { get; set; }
        public bool IsProvidedByImporter { get; set; }
        public bool HasDisposalPortion { get; set; }
        public decimal PercentageRecoverable { get; set; }
        public string MethodOfDisposal { get; set; }
        public ValuePerWeightUnits EstimatedUnit { get; set; }
        public ValuePerWeightUnits CostUnit { get; set; }
        public ValuePerWeightUnits DisposalUnit { get; set; }
        public decimal EstimatedAmount { get; set; }
        public decimal CostAmount { get; set; }
        public decimal DisposalAmount { get; set; }
        public bool IsWasteRecoveryInformationCompleted { get; set; }

        public WasteRecoveryViewModel()
        {
        }

        public WasteRecoveryViewModel(Guid notificationId,
            WasteRecoveryOverview wasteRecovery, 
            WasteDisposalOverview wasteDisposal, 
            NotificationApplicationCompletionProgress progress)
        {
            IsWasteRecoveryInformationCompleted = progress.HasRecoveryData;
            IsProvidedByImporter = progress.HasRecoveryData;
            NotificationId = notificationId;

            if (wasteRecovery != null)
            {
                IsProvidedByImporter = false;
                HasDisposalPortion = wasteRecovery.PercentageRecoverable != 100;
                PercentageRecoverable = wasteRecovery.PercentageRecoverable;
                EstimatedUnit = wasteRecovery.EstimatedValue.Unit;
                EstimatedAmount = wasteRecovery.EstimatedValue.Amount;
                CostUnit = wasteRecovery.RecoveryCost.Unit;
                CostAmount = wasteRecovery.RecoveryCost.Amount;

                if (HasDisposalPortion && wasteDisposal != null)
                {
                    MethodOfDisposal = wasteDisposal.DisposalMethod;
                    DisposalUnit = wasteDisposal.DisposalCost.Unit;
                    DisposalAmount = wasteDisposal.DisposalCost.Amount;
                }
            }
        }
    }
}