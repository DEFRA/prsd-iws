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

        private List<WasteTypeCompositionData> wasteComposition = new List<WasteTypeCompositionData>();
        public List<WasteTypeCompositionData> WasteComposition
        {
            get
            {
                wasteComposition.Sort((x, y) => x.ChemicalCompositionCategory.CompareTo(y.ChemicalCompositionCategory));
                return wasteComposition;
            }
            set { wasteComposition = value; }
        }

        public List<WasteTypeCompositionData> OtherCodes { get; set; }

        public string Command { get; set; }

        [StringLength(70, ErrorMessage = "Please limit your answer to 70 characters or less")]
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
                yield return new ValidationResult("Description is required", new[] { "Description" });
            }

            for (var i = 0; i < WasteComposition.Count; i++)
            {
                if (IsDecimal(WasteComposition[i].MinConcentration) && IsDecimal(WasteComposition[i].MaxConcentration))
                {
                    var minConcentrationValue = Convert.ToDecimal(WasteComposition[i].MinConcentration);
                    var maxConcentrationValue = Convert.ToDecimal(WasteComposition[i].MaxConcentration);

                    if (minConcentrationValue > maxConcentrationValue)
                    {
                        yield return new ValidationResult("Min concentration should be lower than the Max concentration - "
                            + EnumHelper.GetDescription(WasteComposition[i].ChemicalCompositionCategory), new[] { "WasteComposition[" + i + "]" });
                    }

                    if (maxConcentrationValue < 0 || maxConcentrationValue > 100)
                    {
                        yield return new ValidationResult("Max concentration should be in range from 0 to 100 for "
                            + EnumHelper.GetDescription(WasteComposition[i].ChemicalCompositionCategory), new[] { "WasteComposition[" + i + "]" });
                    }
                }

                if (string.IsNullOrEmpty(WasteComposition[i].MinConcentration) || string.IsNullOrEmpty(WasteComposition[i].MaxConcentration))
                {
                    yield return new ValidationResult("Please enter a Min and Max concentration for " 
                        + EnumHelper.GetDescription(WasteComposition[i].ChemicalCompositionCategory), new[] { "WasteComposition[" + i + "]" });
                }
                else if ((!IsDecimal(WasteComposition[i].MinConcentration) && !WasteComposition[i].MinConcentration.ToUpper().Equals("NA")) 
                    || (!IsDecimal(WasteComposition[i].MaxConcentration) && !WasteComposition[i].MaxConcentration.ToUpper().Equals("NA")))
                {
                    yield return new ValidationResult("Please enter a Min and Max concentration for " 
                        + EnumHelper.GetDescription(WasteComposition[i].ChemicalCompositionCategory), new[] { "WasteComposition[" + i + "]" });
                }

                if (!string.IsNullOrEmpty(WasteComposition[i].MinConcentration) && !string.IsNullOrEmpty(WasteComposition[i].MaxConcentration))
                {
                    if ((WasteComposition[i].MinConcentration.ToUpper().Equals("NA") && !WasteComposition[i].MaxConcentration.ToUpper().Equals("NA"))
                    || (!WasteComposition[i].MinConcentration.ToUpper().Equals("NA") && WasteComposition[i].MaxConcentration.ToUpper().Equals("NA")))
                    {
                        yield return new ValidationResult("Both fields must either contain 'NA' or a value - "
                            + EnumHelper.GetDescription(WasteComposition[i].ChemicalCompositionCategory), new[] { "WasteComposition[" + i + "]" });
                    }
                }
            }

            for (var i = 0; i < OtherCodes.Count; i++)
            {
                if (IsDecimal(OtherCodes[i].MinConcentration) && IsDecimal(OtherCodes[i].MaxConcentration))
                {
                    var minConcentrationValue = Convert.ToDecimal(OtherCodes[i].MinConcentration);
                    var maxConcentrationValue = Convert.ToDecimal(OtherCodes[i].MaxConcentration);

                    if (minConcentrationValue < 0 || minConcentrationValue > 100)
                    {
                        yield return new ValidationResult("Min concentration should be in range from 0 to 100 for " + EnumHelper.GetDescription(OtherCodes[i].ChemicalCompositionCategory), new[] { "OtherCodes[" + i + "]" });
                    }

                    if (maxConcentrationValue < 0 || maxConcentrationValue > 100)
                    {
                        yield return new ValidationResult("Max concentration should be in range from 0 to 100 for " + EnumHelper.GetDescription(OtherCodes[i].ChemicalCompositionCategory), new[] { "OtherCodes[" + i + "]" });
                    }

                    if (minConcentrationValue > maxConcentrationValue)
                    {
                        yield return new ValidationResult("Min concentration should be lower than the Max concentration - " + EnumHelper.GetDescription(OtherCodes[i].ChemicalCompositionCategory), new[] { "OtherCodes[" + i + "]" });
                    }
                }

                if (!string.IsNullOrEmpty(OtherCodes[i].Constituent) && (string.IsNullOrEmpty(OtherCodes[i].MinConcentration) || string.IsNullOrEmpty(OtherCodes[i].MaxConcentration)))
                {
                    yield return new ValidationResult("Please enter a Min and Max concentration for " + EnumHelper.GetDescription(OtherCodes[i].ChemicalCompositionCategory), new[] { "OtherCodes[" + i + "]" });
                }

                if (string.IsNullOrEmpty(OtherCodes[i].Constituent) && (!string.IsNullOrEmpty(OtherCodes[i].MinConcentration) || !string.IsNullOrEmpty(OtherCodes[i].MaxConcentration)))
                {
                    yield return new ValidationResult("Please enter a name for the 'Other' component " + EnumHelper.GetDescription(OtherCodes[i].ChemicalCompositionCategory), new[] { "OtherCodes[" + i + "]" });
                }

                if (!string.IsNullOrEmpty(OtherCodes[i].MinConcentration) && !string.IsNullOrEmpty(OtherCodes[i].MaxConcentration))
                {
                    if ((OtherCodes[i].MinConcentration.ToUpper().Equals("NA") && !OtherCodes[i].MaxConcentration.ToUpper().Equals("NA"))
                        || (!OtherCodes[i].MinConcentration.ToUpper().Equals("NA") && OtherCodes[i].MaxConcentration.ToUpper().Equals("NA")))
                    {
                        yield return new ValidationResult("Both fields must either contain 'NA' or a value - "
                            + EnumHelper.GetDescription(OtherCodes[i].ChemicalCompositionCategory), new[] { "OtherCodes[" + i + "]" });
                    }
                }
            }

            var totalNas = 0;
            var totalNotEmpty = 0;

            foreach (WasteTypeCompositionData i in allCompositions)
            {
                if (!string.IsNullOrEmpty(i.MinConcentration) && !string.IsNullOrEmpty(i.MaxConcentration))
                {
                    totalNotEmpty = totalNotEmpty + 1;

                    if (i.MinConcentration.ToUpper().Equals("NA") && i.MaxConcentration.ToUpper().Equals("NA"))
                    {
                        totalNas = totalNas + 1;
                    }
                }
            }

            if (totalNas == totalNotEmpty)
            {
                yield return new ValidationResult("You’ve not entered any data about the waste’s chemical composition. Please make sure you enter the concentration levels for at least one component.");
            }
        }

        private static bool IsDecimal(string input)
        {
            double value;
            return Double.TryParse(input, out value);
        }
    }
}