namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteRecovery
{
    using Core.Shared;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using Prsd.Core.Helpers;
    using Requests.WasteRecovery;

    public class DisposalCostViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }

        [Required(ErrorMessage = "Please enter the amount in GBP(£) for cost of disposal")]
        [Display(Name = "Please enter £/kg or tonne")]
        public string Amount { get; set; }

        public ValuePerWeightUnits Units { get; set; }

        public SelectList PossibleUnits
        {
            get
            {
                var units = Enum.GetValues(typeof(ValuePerWeightUnits)).Cast<ValuePerWeightUnits>().Select(u => new
                {
                    Key = EnumHelper.GetDisplayName(u),
                    Value = (int)u
                });

                return new SelectList(units, "Value", "Key", Units);
            }
        }

        public DisposalCostViewModel()
        {
        }

        public DisposalCostViewModel(Guid id, ValuePerWeightData data)
        {
            NotificationId = id;
            Amount = data.Amount.ToString(CultureInfo.InvariantCulture);
            Units = data.Unit;
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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (Units == 0)
            {
                results.Add(new ValidationResult("Please select the units"));
            }

            if (!IsCostValid(Amount))
            {
                results.Add(new ValidationResult("The amount that you have entered does not seem to be valid, it needs to be a number with no more than two decimal places and can have a comma as a thousand separator, please see the examples.", new[] { "Amount" }));
            }

            return results;
        }
    }
}