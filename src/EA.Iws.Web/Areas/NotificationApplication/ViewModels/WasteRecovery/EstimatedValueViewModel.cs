namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteRecovery
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Shared;
    using Infrastructure;
    using Infrastructure.Attributes;
    using Prsd.Core.Helpers;
    using Requests.WasteRecovery;

    public class EstimatedValueViewModel : IValidatableObject
    {
        public decimal PercentageRecoverable { get; set; }

        [Required(ErrorMessage = "Please enter a value")]
        [IsValidNumber(maxPrecision: 12, allowNegative: false)]
        [IsValidMoneyDecimal]
        [Display(Name = "Please enter £/kg or tonne")]
        public string Amount { get; set; }

        [Required]
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
            if (!SelectedUnits.HasValue)
            {
                yield return new ValidationResult("Please select a unit", new[] { "SelectedUnits" });
            }
        }
    }
}