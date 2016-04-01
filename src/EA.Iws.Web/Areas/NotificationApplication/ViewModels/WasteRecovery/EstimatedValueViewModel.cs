namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteRecovery
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Shared;
    using Infrastructure.Validation;
    using Prsd.Core.Helpers;
    using Views.WasteRecovery;

    public class EstimatedValueViewModel
    {
        public decimal PercentageRecoverable { get; set; }

        public ValuePerWeightUnits ShipmentInfoUnits { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(EstimatedValueResources))]
        [Required(ErrorMessageResourceName = "AmountRequired", ErrorMessageResourceType = typeof(EstimatedValueResources))]
        [IsValidNumber(maxPrecision: 12)]
        [IsValidMoneyDecimal]
        public string Amount { get; set; }

        [Display(Name = "SelectedUnits", ResourceType = typeof(EstimatedValueResources))]
        [Required(ErrorMessageResourceName = "UnitsRequired", ErrorMessageResourceType = typeof(EstimatedValueResources))]
        public ValuePerWeightUnits? SelectedUnits { get; set; }

        public SelectList UnitSelectList
        {
            get
            {
                var units = Enum.GetValues(typeof(ValuePerWeightUnits))
                    .Cast<ValuePerWeightUnits>()
                    .Select(u => new
                    {
                        Key = EnumHelper.GetDisplayName(u),
                        Value = (int)u
                    }).ToList();

                return new SelectList(units, "Value", "Key", SelectedUnits);
            }
        }

        public EstimatedValueViewModel()
        {
        }

        public EstimatedValueViewModel(decimal percentage, ValuePerWeightData estimatedValueData, ValuePerWeightUnits units)
        {
            PercentageRecoverable = percentage;
            ShipmentInfoUnits = units;
            
            if (estimatedValueData != null)
            {
                Amount = estimatedValueData.Amount.ToString();
                SelectedUnits = estimatedValueData.Unit;
            }
        }

        public EstimatedValueViewModel(decimal percentage, ValuePerWeightUnits units)
        {
            PercentageRecoverable = percentage;
            ShipmentInfoUnits = units;

            Amount = string.Empty;
            SelectedUnits = units;
        }
    }
}