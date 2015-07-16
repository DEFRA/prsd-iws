namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.RecoveryInfo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;
    using Core.RecoveryInfo;
    using Microsoft.Ajax.Utilities;
    using Prsd.Core.Helpers;
    using Prsd.Core.Validation;
    using Requests.RecoveryInfo;
    using Web.ViewModels.Shared;

    public class RecoveryInfoValuesViewModel : IValidatableObject
    {
        public Guid Id { get; set; }

        public Guid NotificationId { get; set; }

        public bool IsDisposal { get; set; }

        public RadioButtonStringCollectionViewModel EstimatedUnit { get; set; }

        public RadioButtonStringCollectionViewModel CostUnit { get; set; }

        public RadioButtonStringCollectionOptionalViewModel DisposalUnit { get; set; }

        [Required(ErrorMessage = "Please enter the amount in GBP(£) for estimated value of the recoverable material.")]
        [Display(Name = "Enter the amount in GBP(£)")]
        public string EstimatedAmount { get; set; }

        [Required(ErrorMessage = "Please enter the amount in GBP(£) for cost of recovery.")]
        [Display(Name = "Enter the amount in GBP(£)")]
        public string CostAmount { get; set; }

        [Display(Name = "Enter the amount in GBP(£)")]
        public string DisposalAmount { get; set; }

        public AddRecoveryInfoToNotification ToRequest()
        {
            RecoveryInfoUnits estimatedUnit = (EstimatedUnit.SelectedValue == EnumHelper.GetDisplayName(RecoveryInfoUnits.Kilogram))
                                                ? RecoveryInfoUnits.Kilogram : RecoveryInfoUnits.Tonne;

            RecoveryInfoUnits costUnit = (CostUnit.SelectedValue == EnumHelper.GetDisplayName(RecoveryInfoUnits.Kilogram))
                                                ? RecoveryInfoUnits.Kilogram : RecoveryInfoUnits.Tonne;

            if (IsDisposal)
            {
                RecoveryInfoUnits disposalUnit = (DisposalUnit.SelectedValue == EnumHelper.GetDisplayName(RecoveryInfoUnits.Kilogram))
                                                    ? RecoveryInfoUnits.Kilogram : RecoveryInfoUnits.Tonne;

                return new AddRecoveryInfoToNotification(NotificationId, IsDisposal,
                            estimatedUnit, Convert.ToDecimal(EstimatedAmount),
                            costUnit, Convert.ToDecimal(CostAmount),
                            disposalUnit, Convert.ToDecimal(DisposalAmount));
            }

            return new AddRecoveryInfoToNotification(NotificationId, IsDisposal,
                        estimatedUnit, Convert.ToDecimal(EstimatedAmount),
                        costUnit, Convert.ToDecimal(CostAmount), null, null);
        }

        public RecoveryInfoValuesViewModel()
        {
            EstimatedUnit = RadioButtonStringCollectionViewModel.CreateFromEnum<RecoveryInfoUnits>();
            CostUnit = RadioButtonStringCollectionViewModel.CreateFromEnum<RecoveryInfoUnits>();
            DisposalUnit = RadioButtonStringCollectionOptionalViewModel.CreateFromEnum<RecoveryInfoUnits>();
        }

        public RecoveryInfoValuesViewModel(RecoveryInfoData recoveryInfoData)
        {
            EstimatedUnit = RadioButtonStringCollectionViewModel.CreateFromEnum<RecoveryInfoUnits>();
            CostUnit = RadioButtonStringCollectionViewModel.CreateFromEnum<RecoveryInfoUnits>();
            DisposalUnit = RadioButtonStringCollectionOptionalViewModel.CreateFromEnum<RecoveryInfoUnits>();

            EstimatedAmount = recoveryInfoData.EstimatedAmount.ToString();
            CostAmount = recoveryInfoData.CostAmount.ToString();
            DisposalAmount = recoveryInfoData.DisposalAmount.ToString();
            EstimatedUnit.SelectedValue = recoveryInfoData.EstimatedUnit != null ? EnumHelper.GetDisplayName(recoveryInfoData.EstimatedUnit) : null;
            CostUnit.SelectedValue = recoveryInfoData.CostUnit != null ? EnumHelper.GetDisplayName(recoveryInfoData.CostUnit) : null;
            DisposalUnit.SelectedValue = recoveryInfoData.DisposalUnit != null ? EnumHelper.GetDisplayName(recoveryInfoData.DisposalUnit) : null;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            if (IsDisposal)
            {
                if (String.IsNullOrWhiteSpace(DisposalUnit.SelectedValue))
                {
                    results.Add(new ValidationResult("Please answer this question.", new[] { "DisposalUnit.SelectedValue" }));
                }

                if (!IsCostValid(DisposalAmount))
                {
                    results.Add(new ValidationResult("Please enter a valid disposal amount in GBP(£)", new[] { "DisposalAmount" }));
                }
            }

            if (!IsCostValid(EstimatedAmount))
            {
                results.Add(new ValidationResult("Please enter a valid estimated amount in GBP(£)", new[] { "EstimatedAmount" }));
            }

            if (!IsCostValid(CostAmount))
            {
                results.Add(new ValidationResult("Please enter a valid cost amount in GBP(£)", new[] { "CostAmount" }));
            }

            return results;
        }

        private bool IsCostValid(string amount)
        {
            if (string.IsNullOrWhiteSpace(amount))
            {
                return false;
            }

            decimal cost;
            if (amount.Contains(","))
            {   
                Regex rgx = new Regex(@"^(?=[\d.])\d{0,3}(?:\d*|(?:,\d{3})*)(?:\.\d{1,2})?$");
                if (rgx.IsMatch(amount))
                {
                    amount = amount.Replace(",", string.Empty);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                Regex rgx = new Regex(@"^[-]?\d+(?:\.\d{1,2})?$");
                if (!rgx.IsMatch(amount))
                {
                    return false;
                }
            }

            if (!Decimal.TryParse(amount, out cost))
            {
                return false;
            }

            if (cost < 0)
            {
                return false;
            }
            return true;
        }
    }
}