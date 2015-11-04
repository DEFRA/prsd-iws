namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.ChemicalComposition
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Core.WasteType;
    using Prsd.Core.Helpers;
    using Requests.WasteType;
    using Views.ChemicalComposition;

    public class ChemicalCompositionContinuedViewModel : IValidatableObject
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

        public string FurtherInformation { get; set; }

        public bool HasAnnex { get; set; }

        public string Command { get; set; }

        public ChemicalCompositionType ChemicalCompositionType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Command.Equals("add"))
            {
                yield break;
            }

            var allCompositions = WasteComposition.Concat(OtherCodes).ToList();

            for (var i = 0; i < WasteComposition.Count; i++)
            {
                if (IsDecimal(WasteComposition[i].MinConcentration) && IsDecimal(WasteComposition[i].MaxConcentration))
                {
                    var minConcentrationValue = Convert.ToDecimal(WasteComposition[i].MinConcentration);
                    var maxConcentrationValue = Convert.ToDecimal(WasteComposition[i].MaxConcentration);

                    if (minConcentrationValue > maxConcentrationValue)
                    {
                        yield return new ValidationResult(string.Format(ChemicalCompositionResources.MinShouldBeLowerThanMax,
                            EnumHelper.GetDescription(WasteComposition[i].ChemicalCompositionCategory)), new[] { "WasteComposition[" + i + "]" });
                    }

                    if (maxConcentrationValue < 0 || maxConcentrationValue > 100)
                    {
                        yield return new ValidationResult(string.Format(ChemicalCompositionResources.MaxRange,
                            EnumHelper.GetDescription(WasteComposition[i].ChemicalCompositionCategory)), new[] { "WasteComposition[" + i + "]" });
                    }

                    if (minConcentrationValue < 0 || minConcentrationValue > 100)
                    {
                        yield return new ValidationResult(string.Format(ChemicalCompositionResources.MinRange,
                            EnumHelper.GetDescription(WasteComposition[i].ChemicalCompositionCategory)), new[] { "WasteComposition[" + i + "]" });
                    }
                }

                if (string.IsNullOrEmpty(WasteComposition[i].MinConcentration) || string.IsNullOrEmpty(WasteComposition[i].MaxConcentration))
                {
                    yield return new ValidationResult(string.Format(ChemicalCompositionResources.MinMaxRequired,
                        EnumHelper.GetDescription(WasteComposition[i].ChemicalCompositionCategory)), new[] { "WasteComposition[" + i + "]" });
                }
                else if ((!IsDecimal(WasteComposition[i].MinConcentration) && !WasteComposition[i].MinConcentration.ToUpper().Equals(ChemicalCompositionResources.NA))
                    || (!IsDecimal(WasteComposition[i].MaxConcentration) && !WasteComposition[i].MaxConcentration.ToUpper().Equals(ChemicalCompositionResources.NA)))
                {
                    yield return new ValidationResult(string.Format(ChemicalCompositionResources.MinMaxValid,
                        EnumHelper.GetDescription(WasteComposition[i].ChemicalCompositionCategory)), new[] { "WasteComposition[" + i + "]" });
                }

                if (!string.IsNullOrEmpty(WasteComposition[i].MinConcentration) && !string.IsNullOrEmpty(WasteComposition[i].MaxConcentration))
                {
                    if ((WasteComposition[i].MinConcentration.ToUpper().Equals(ChemicalCompositionResources.NA) && !WasteComposition[i].MaxConcentration.ToUpper().Equals(ChemicalCompositionResources.NA))
                    || (!WasteComposition[i].MinConcentration.ToUpper().Equals(ChemicalCompositionResources.NA) && WasteComposition[i].MaxConcentration.ToUpper().Equals(ChemicalCompositionResources.NA)))
                    {
                        yield return new ValidationResult(string.Format(ChemicalCompositionResources.FieldShouldHaveNaOrValue,
                            EnumHelper.GetDescription(WasteComposition[i].ChemicalCompositionCategory)), new[] { "WasteComposition[" + i + "]" });
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
                        yield return new ValidationResult(string.Format(ChemicalCompositionResources.MinRange, 
                            EnumHelper.GetDescription(OtherCodes[i].ChemicalCompositionCategory)), new[] { "OtherCodes[" + i + "]" });
                    }

                    if (maxConcentrationValue < 0 || maxConcentrationValue > 100)
                    {
                        yield return new ValidationResult(string.Format(ChemicalCompositionResources.MaxRange, 
                            EnumHelper.GetDescription(OtherCodes[i].ChemicalCompositionCategory)), new[] { "OtherCodes[" + i + "]" });
                    }

                    if (minConcentrationValue > maxConcentrationValue)
                    {
                        yield return new ValidationResult(string.Format(ChemicalCompositionResources.MinShouldBeLowerThanMax, 
                            EnumHelper.GetDescription(OtherCodes[i].ChemicalCompositionCategory)), new[] { "OtherCodes[" + i + "]" });
                    }
                }

                if (!string.IsNullOrEmpty(OtherCodes[i].Constituent) && (string.IsNullOrEmpty(OtherCodes[i].MinConcentration) || string.IsNullOrEmpty(OtherCodes[i].MaxConcentration)))
                {
                    yield return new ValidationResult(string.Format(ChemicalCompositionResources.MinMaxRequired, 
                        EnumHelper.GetDescription(OtherCodes[i].ChemicalCompositionCategory)), new[] { "OtherCodes[" + i + "]" });
                }

                if (string.IsNullOrEmpty(OtherCodes[i].Constituent) && (!string.IsNullOrEmpty(OtherCodes[i].MinConcentration) || !string.IsNullOrEmpty(OtherCodes[i].MaxConcentration)))
                {
                    yield return new ValidationResult(string.Format(ChemicalCompositionResources.OtherNameRequired, 
                        EnumHelper.GetDescription(OtherCodes[i].ChemicalCompositionCategory)), new[] { "OtherCodes[" + i + "]" });
                }

                if (!string.IsNullOrEmpty(OtherCodes[i].MinConcentration) && !string.IsNullOrEmpty(OtherCodes[i].MaxConcentration))
                {
                    if ((OtherCodes[i].MinConcentration.ToUpper().Equals(ChemicalCompositionResources.NA) && !OtherCodes[i].MaxConcentration.ToUpper().Equals(ChemicalCompositionResources.NA))
                        || (!OtherCodes[i].MinConcentration.ToUpper().Equals(ChemicalCompositionResources.NA) && OtherCodes[i].MaxConcentration.ToUpper().Equals(ChemicalCompositionResources.NA)))
                    {
                        yield return new ValidationResult(string.Format(ChemicalCompositionResources.FieldShouldHaveNaOrValue,
                            EnumHelper.GetDescription(OtherCodes[i].ChemicalCompositionCategory)), new[] { "OtherCodes[" + i + "]" });
                    }
                }
            }

            if (HasAnnex && !(string.IsNullOrEmpty(FurtherInformation)))
            {
                yield return new ValidationResult(ChemicalCompositionResources.FurtherInfoValidation, new[] { "FurtherInformation" });
            }

            var totalNas = 0;
            var totalNotEmpty = 0;

            foreach (WasteTypeCompositionData i in allCompositions)
            {
                if (!string.IsNullOrEmpty(i.MinConcentration) && !string.IsNullOrEmpty(i.MaxConcentration))
                {
                    totalNotEmpty = totalNotEmpty + 1;

                    if (i.MinConcentration.ToUpper().Equals(ChemicalCompositionResources.NA)
                        && i.MaxConcentration.ToUpper().Equals(ChemicalCompositionResources.NA))
                    {
                        totalNas = totalNas + 1;
                    }
                }
            }

            if (totalNas == totalNotEmpty)
            {
                yield return new ValidationResult(ChemicalCompositionResources.WasteDataRequired);
            }
        }

        private static bool IsDecimal(string input)
        {
            double value;
            return Double.TryParse(input, out value);
        }
    }
}