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
    using Requests.WasteRecovery;

    public class EstimatedValueViewModel : IValidatableObject
    {
        public decimal PercentageRecoverable { get; set; }

        [Display(Name = "Please enter £/kg or tonne")]
        [Required(ErrorMessage = "Please enter the amount in GBP(£) for the estimated value")]
        [IsValidNumber(maxPrecision: 12)]
        [IsValidMoneyDecimal]
        public string Amount { get; set; }

        [Required(ErrorMessage = "Please select the units")]
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
                yield return new ValidationResult("The amount entered cannot be negative", new[] { "Amount" });
            }
        }
    }
}