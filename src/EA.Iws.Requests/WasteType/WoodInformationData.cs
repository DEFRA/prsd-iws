namespace EA.Iws.Requests.WasteType
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.Shared;
    using Core.WasteType;
    using Prsd.Core.Helpers;

    public class WoodInformationData : ChemicalCompositionData, IValidatableObject
    {
        public WasteInformationType WasteInformationType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(MinConcentration) || string.IsNullOrEmpty(MaxConcentration))
            {
                yield return new ValidationResult("Please enter a Min and Max concentration for " + EnumHelper.GetDescription(WasteInformationType));
            }
            else if ((!IsDecimal(MinConcentration) && !MinConcentration.ToUpper().Equals("NA")) || (!IsDecimal(MaxConcentration) && !MaxConcentration.ToUpper().Equals("NA")))
            {
                yield return new ValidationResult("Please enter a valid Min and Max concentration for " + EnumHelper.GetDescription(WasteInformationType));
            }

            if (IsDecimal(MinConcentration) && IsDecimal(MaxConcentration))
            {
                var minConcentrationValue = Convert.ToDecimal(MinConcentration);
                var maxConcentrationValue = Convert.ToDecimal(MaxConcentration);

                if (minConcentrationValue < 0 || minConcentrationValue > 100)
                {
                    yield return new ValidationResult("Min concentration should be in range from 0 to 100 for  " + EnumHelper.GetDescription(WasteInformationType));
                }
                if (maxConcentrationValue < 0 || maxConcentrationValue > 100)
                {
                    yield return new ValidationResult("Max concentration should be in range from 0 to 100 for  " + EnumHelper.GetDescription(WasteInformationType));
                }
                if (minConcentrationValue > maxConcentrationValue)
                {
                    yield return new ValidationResult("Min concentration should lower than the Max concentration  " + EnumHelper.GetDescription(WasteInformationType));
                }
            }
        }

        private static bool IsDecimal(string input)
        {
            double value;
            return Double.TryParse(input, out value);
        }
    }
}