namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteRecovery
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Shared;
    using Infrastructure;
    using Infrastructure.Validation;
    using Prsd.Core.Helpers;
    using Views.WasteRecovery;

    public class EstimatedValueViewModel : IValidatableObject
    {
        public decimal PercentageRecoverable { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(EstimatedValueResources))]
        [Required(ErrorMessageResourceName = "AmountRequired", ErrorMessageResourceType = typeof(EstimatedValueResources))]
        [IsValidNumber(maxPrecision: 12)]
        [IsValidMoneyDecimal]
        public string Amount { get; set; }

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

        public EstimatedValueViewModel(decimal percentage, ValuePerWeightData estimatedValueData)
        {
            PercentageRecoverable = percentage;

            if (estimatedValueData != null)
            {
                Amount = estimatedValueData.Amount.ToString();
                SelectedUnits = estimatedValueData.Unit;
            }
        }
                
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Amount.ToMoneyDecimal() < 0)
            {
                yield return new ValidationResult(EstimatedValueResources.AmountCannotBeNegative, new[] { "Amount" });
            }
        }
    }
}