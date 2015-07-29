namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteType
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.WasteType;
    using Prsd.Core.Helpers;

    public class ChemicalCompositionInformationViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }

        public List<WoodInformationData> WasteComposition { get; set; }

        public string FurtherInformation { get; set; }

        public bool HasAnnex { get; set; }

        public string Energy { get; set; }

        public ChemicalCompositionType ChemicalCompositionType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Energy) && ChemicalCompositionType != ChemicalCompositionType.Wood)
            {
                yield return new ValidationResult("Please enter a value for Energy", new[] { "Energy" });
            }

            for (var i = 0; i < WasteComposition.Count; i++)
            {
                if (string.IsNullOrEmpty(WasteComposition[i].MinConcentration) || string.IsNullOrEmpty(WasteComposition[i].MaxConcentration))
                {
                    yield return new ValidationResult("Please enter a Min and Max concentration for " + EnumHelper.GetDescription(WasteComposition[i].WasteInformationType), new[] { "WasteComposition[" + i + "]" });
                }
                else if ((!IsDecimal(WasteComposition[i].MinConcentration) && !WasteComposition[i].MinConcentration.ToUpper().Equals("NA")) || (!IsDecimal(WasteComposition[i].MaxConcentration) && !WasteComposition[i].MaxConcentration.ToUpper().Equals("NA")))
                {
                    yield return new ValidationResult("Please enter a valid Min and Max concentration for " + EnumHelper.GetDescription(WasteComposition[i].WasteInformationType), new[] { "WasteComposition[" + i + "]" });
                }

                if (IsDecimal(WasteComposition[i].MinConcentration) && IsDecimal(WasteComposition[i].MaxConcentration))
                {
                    var minConcentrationValue = Convert.ToDecimal(WasteComposition[i].MinConcentration);
                    var maxConcentrationValue = Convert.ToDecimal(WasteComposition[i].MaxConcentration);

                    if (minConcentrationValue < 0 || minConcentrationValue > 100)
                    {
                        yield return new ValidationResult("Min concentration should be in range from 0 to 100 for  " + EnumHelper.GetDescription(WasteComposition[i].WasteInformationType), new[] { "WasteComposition[" + i + "]" });
                    }
                    if (maxConcentrationValue < 0 || maxConcentrationValue > 100)
                    {
                        yield return new ValidationResult("Max concentration should be in range from 0 to 100 for  " + EnumHelper.GetDescription(WasteComposition[i].WasteInformationType), new[] { "WasteComposition[" + i + "]" });
                    }
                    if (minConcentrationValue > maxConcentrationValue)
                    {
                        yield return new ValidationResult("Min concentration should be lower than the Max concentration  " + EnumHelper.GetDescription(WasteComposition[i].WasteInformationType), new[] { "WasteComposition[" + i + "]" });
                    }
                }
            }

            if (HasAnnex && !(string.IsNullOrEmpty(FurtherInformation)))
            {
                yield return new ValidationResult("If you select that you are providing the details in a separate annex do not enter any details here.", new[] { "FurtherInformation" });
            }
        }

        private static bool IsDecimal(string input)
        {
            double value;
            return Double.TryParse(input, out value);
        }
    }
}