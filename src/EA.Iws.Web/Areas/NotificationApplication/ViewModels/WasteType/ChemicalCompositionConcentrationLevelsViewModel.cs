namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteType
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Core.WasteType;
    using Prsd.Core.Helpers;
    using Requests.WasteType;

    public class ChemicalCompositionConcentrationLevelsViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }

        public List<WasteTypeCompositionData> WasteComposition { get; set; }

        public List<WasteTypeCompositionData> OtherCodes { get; set; }

        public string Command { get; set; }

        public string Description { get; set; }

        public ChemicalCompositionType ChemicalCompositionType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Command.Equals("add"))
            {
                yield break;
            }

            var allCompositions = WasteComposition.Concat(OtherCodes).ToList();

            if (ChemicalCompositionType == ChemicalCompositionType.Wood && string.IsNullOrEmpty(Description))
            {
                yield return new ValidationResult("Description is required");
            }

            for (var i = 0; i < OtherCodes.Count; i++)
            {
                if (!string.IsNullOrEmpty(OtherCodes[i].Constituent) && (string.IsNullOrEmpty(OtherCodes[i].MinConcentration) || string.IsNullOrEmpty(OtherCodes[i].MaxConcentration)))
                {
                    yield return new ValidationResult("Please enter a Min and Max concentration for " + EnumHelper.GetDescription(OtherCodes[i].ChemicalCompositionCategory), new[] { "OtherCodes[" + i + "]" });
                }
                if (string.IsNullOrEmpty(OtherCodes[i].Constituent) && (!string.IsNullOrEmpty(OtherCodes[i].MinConcentration) || !string.IsNullOrEmpty(OtherCodes[i].MaxConcentration)))
                {
                    yield return new ValidationResult("Please enter a name for the 'Other' component " + EnumHelper.GetDescription(OtherCodes[i].ChemicalCompositionCategory), new[] { "OtherCodes[" + i + "]" });
                }
            }

            for (var i = 0; i < WasteComposition.Count; i++)
            {
                if (string.IsNullOrEmpty(WasteComposition[i].MinConcentration) || string.IsNullOrEmpty(WasteComposition[i].MaxConcentration))
                {
                    yield return new ValidationResult("Please enter a Min and Max concentration for " + EnumHelper.GetDescription(WasteComposition[i].ChemicalCompositionCategory), new[] { "WasteComposition[" + i + "]" });
                }
                else if ((!IsDecimal(WasteComposition[i].MinConcentration) && !WasteComposition[i].MinConcentration.ToUpper().Equals("NA")) || (!IsDecimal(WasteComposition[i].MaxConcentration) && !WasteComposition[i].MaxConcentration.ToUpper().Equals("NA")))
                {
                    yield return new ValidationResult("Please enter  valid Min and Max concentration for " + EnumHelper.GetDescription(WasteComposition[i].ChemicalCompositionCategory), new[] { "WasteComposition[" + i + "]" });
                }
            }

            for (var i = 0; i < allCompositions.Count; i++)
            {
                if (IsDecimal(allCompositions[i].MinConcentration) && IsDecimal(allCompositions[i].MaxConcentration))
                {
                    var minConcentrationValue = Convert.ToDecimal(allCompositions[i].MinConcentration);
                    var maxConcentrationValue = Convert.ToDecimal(allCompositions[i].MaxConcentration);

                    if (minConcentrationValue < 0 || minConcentrationValue > 100)
                    {
                        yield return new ValidationResult("Min concentration should be in range from 0 to 100 for  " + EnumHelper.GetDescription(allCompositions[i].ChemicalCompositionCategory));
                    }
                    if (maxConcentrationValue < 0 || maxConcentrationValue > 100)
                    {
                        yield return new ValidationResult("Max concentration should be in range from 0 to 100 for  " + EnumHelper.GetDescription(allCompositions[i].ChemicalCompositionCategory));
                    }
                    if (minConcentrationValue > maxConcentrationValue)
                    {
                        yield return new ValidationResult("Min concentration should be lower than the Max concentration  " + EnumHelper.GetDescription(allCompositions[i].ChemicalCompositionCategory));
                    }
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