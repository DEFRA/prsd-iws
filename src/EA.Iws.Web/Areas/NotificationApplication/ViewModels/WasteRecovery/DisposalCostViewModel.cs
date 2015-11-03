namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteRecovery
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Shared;
    using Infrastructure;
    using Infrastructure.Validation;
    using Prsd.Core.Helpers;
    using Views.WasteRecovery;

    public class DisposalCostViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(DisposalCostResources))]
        [Required(ErrorMessageResourceName = "AmountRequired", ErrorMessageResourceType = typeof(DisposalCostResources))]
        [IsValidNumber(maxPrecision: 12)]
        [IsValidMoneyDecimal]
        public string Amount { get; set; }

        public ValuePerWeightUnits Units { get; set; }

        public string DisposalMethod { get; set; }

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
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (Units == 0)
            {
                results.Add(new ValidationResult(DisposalCostResources.UnitsRequired));
            }

            if (Amount.ToMoneyDecimal() < 0)
            {
                results.Add(new ValidationResult(DisposalCostResources.AmountCannotBeNegative, new[] { "Amount" }));
            }

            return results;
        }
    }
}