namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteRecovery
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using Core.Shared;
    using Prsd.Core.Helpers;
    using Requests.WasteRecovery;

    public class EstimatedValueViewModel : IValidatableObject
    {
        public decimal PercentageRecoverable { get; set; }

        [Required]
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

        public ValuePerWeightData GetEstimatedValue()
        {
            var amount = Convert.ToDecimal(Amount);

            return new ValuePerWeightData(amount, SelectedUnits.Value);
        }

        public bool IsAmountValid()
        {
            decimal amount;
            string amountString = Amount;

            if (amountString.Contains(","))
            {
                Regex rgx = new Regex(@"^(?=[\d.])\d{0,3}(?:\d*|(?:,\d{3})*)(?:\.\d{1,2})?$");
                if (rgx.IsMatch(amountString))
                {
                    amountString = amountString.Replace(",", string.Empty);
                }
                else
                {
                    return false;
                }
            }

            if (!decimal.TryParse(amountString, out amount))
            {
                return false;
            }

            if (amount < 0)
            {
                return false;
            }

            return true;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!IsAmountValid())
            {
                yield return new ValidationResult("Please enter a valid cost amount", new[] { "Amount" });
            }
        }
    }
}