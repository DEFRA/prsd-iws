namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.ChemicalComposition
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.WasteType;
    using Prsd.Core.Helpers;
    using Views.ChemicalComposition;

    public class ChemicalCompositionViewModel : IValidatableObject
    {
        private List<WoodInformationData> wasteComposition = new List<WoodInformationData>();

        public Guid NotificationId { get; set; }

        public List<WoodInformationData> WasteComposition
        {
            get
            {
                wasteComposition.Sort((x, y) =>
                    x.WasteInformationType.CompareTo(y.WasteInformationType));
                return wasteComposition;
            }
            set { wasteComposition = value; }
        }

        public string Energy { get; set; }

        [StringLength(70, ErrorMessageResourceName = "DescriptionLength", ErrorMessageResourceType = typeof(ChemicalCompositionResources))]
        public string Description { get; set; }

        public ChemicalComposition ChemicalCompositionType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Energy) && ChemicalCompositionType != ChemicalComposition.Wood)
            {
                yield return new ValidationResult(ChemicalCompositionResources.EnergyRequired, new[] { "Energy" });
            }

            if (ChemicalCompositionType == ChemicalComposition.Wood && string.IsNullOrEmpty(Description))
            {
                yield return new ValidationResult(ChemicalCompositionResources.DescriptionRequired, new[] { "Description" });
            }

            for (var i = 0; i < WasteComposition.Count; i++)
            {
                if (string.IsNullOrEmpty(WasteComposition[i].MinConcentration) || string.IsNullOrEmpty(WasteComposition[i].MaxConcentration))
                {
                    yield return new ValidationResult(string.Format(ChemicalCompositionResources.MinMaxRequired,
                         EnumHelper.GetDescription(WasteComposition[i].WasteInformationType)), new[] { "WasteComposition[" + i + "]" });
                }
                else if ((!IsDecimal(WasteComposition[i].MinConcentration) && !WasteComposition[i].MinConcentration.ToUpper().Equals(ChemicalCompositionResources.NA))
                    || (!IsDecimal(WasteComposition[i].MaxConcentration) && !WasteComposition[i].MaxConcentration.ToUpper().Equals(ChemicalCompositionResources.NA)))
                {
                    yield return new ValidationResult(string.Format(ChemicalCompositionResources.MinMaxValid,
                        EnumHelper.GetDescription(WasteComposition[i].WasteInformationType)), new[] { "WasteComposition[" + i + "]" });
                }

                if (IsDecimal(WasteComposition[i].MinConcentration) && IsDecimal(WasteComposition[i].MaxConcentration))
                {
                    var minConcentrationValue = Convert.ToDecimal(WasteComposition[i].MinConcentration);
                    var maxConcentrationValue = Convert.ToDecimal(WasteComposition[i].MaxConcentration);

                    if (minConcentrationValue < 0 || minConcentrationValue > 100 && IsPercentageQuantity(WasteComposition[i]))
                    {
                        yield return new ValidationResult(string.Format(ChemicalCompositionResources.MinRange,
                            EnumHelper.GetDescription(WasteComposition[i].WasteInformationType)), new[] { "WasteComposition[" + i + "]" });
                    }
                    if (maxConcentrationValue < 0 || maxConcentrationValue > 100 && IsPercentageQuantity(WasteComposition[i]))
                    {
                        yield return new ValidationResult(string.Format(ChemicalCompositionResources.MaxRange,
                            EnumHelper.GetDescription(WasteComposition[i].WasteInformationType)), new[] { "WasteComposition[" + i + "]" });
                    }
                    if (minConcentrationValue > maxConcentrationValue)
                    {
                        yield return new ValidationResult(string.Format(ChemicalCompositionResources.MinShouldBeLowerThanMax,
                            EnumHelper.GetDescription(WasteComposition[i].WasteInformationType)), new[] { "WasteComposition[" + i + "]" });
                    }
                }

                if (!string.IsNullOrEmpty(WasteComposition[i].MinConcentration) && !string.IsNullOrEmpty(WasteComposition[i].MaxConcentration))
                {
                    if ((WasteComposition[i].MinConcentration.ToUpper().Equals(ChemicalCompositionResources.NA) && !WasteComposition[i].MaxConcentration.ToUpper().Equals(ChemicalCompositionResources.NA))
                        || (!WasteComposition[i].MinConcentration.ToUpper().Equals(ChemicalCompositionResources.NA) && WasteComposition[i].MaxConcentration.ToUpper().Equals(ChemicalCompositionResources.NA)))
                    {
                        yield return new ValidationResult(string.Format(ChemicalCompositionResources.FieldShouldHaveNaOrNumber,
                            EnumHelper.GetDescription(WasteComposition[i].WasteInformationType)), new[] { "WasteComposition[" + i + "]" });
                    }
                }
            }

            var totalNas = 0;
            var totalNotEmpty = 0;

            foreach (WoodInformationData i in WasteComposition)
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
                yield return new ValidationResult(ChemicalCompositionResources.WasteDetailsRequired);
            }
        }

        private static bool IsDecimal(string input)
        {
            double value;
            return Double.TryParse(input, out value);
        }

        private static bool IsPercentageQuantity(WoodInformationData wasteData)
        {
            return wasteData.WasteInformationType != WasteInformationType.HeavyMetals
                   && wasteData.WasteInformationType != WasteInformationType.NetCalorificValue;
        }
    }
}