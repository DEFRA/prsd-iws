namespace EA.Iws.DocumentGeneration.ViewModels
{
    using System.Collections.Generic;
    using Core.WasteType;
    using Domain.NotificationApplication;

    internal class WasteCompositionViewModel
    {
        private const int textLength = 150;

        public string WasteName { get; private set; }

        public bool HasAnnex { get; private set; }
        public string ChemicalCompositionDescription { get; private set; }
        public string ShortDescription { get; private set; }
        public string LongDescription { get; private set; }
        public string Energy { get; private set; }
        public string OptionalInformation { get; private set; }
        public string OtherTypeDescription { get; private set; }
        public string WoodTypeDescription { get; private set; }
        public ChemicalComposition ChemicalComposition { get; private set; }
        public IList<ChemicalCompositionPercentages> Compositions { get; private set; }
        public IList<ChemicalCompositionPercentages> AdditionalInfos { get; private set; }
        public string AnnexMessage { get; private set; }

        public WasteCompositionViewModel(WasteType wasteType)
        {
            WasteName = GetWasteName(wasteType);
            HasAnnex = wasteType.HasAnnex;
            ChemicalCompositionDescription = wasteType.ChemicalCompositionDescription ?? string.Empty;
            ShortDescription = string.Empty;
            LongDescription = string.Empty;
            Energy = wasteType.EnergyInformation ?? string.Empty;
            OptionalInformation = wasteType.OptionalInformation ?? string.Empty;
            OtherTypeDescription = wasteType.OtherWasteTypeDescription ?? string.Empty;
            WoodTypeDescription = wasteType.WoodTypeDescription ?? string.Empty;
            ChemicalComposition = wasteType.ChemicalCompositionType;
            SetCompositions(wasteType);
            SetAdditionalInfos(wasteType);
            AnnexMessage = string.Empty;
            SetMergeDescriptionText();
        }

        public WasteCompositionViewModel(WasteCompositionViewModel model, int annexNumber)
        {
            WasteName = model.WasteName;
            HasAnnex = model.HasAnnex;
            ChemicalCompositionDescription = model.ChemicalCompositionDescription;
            ShortDescription = model.ShortDescription;
            LongDescription = model.LongDescription;
            Energy = model.Energy;
            OptionalInformation = model.OptionalInformation;
            OtherTypeDescription = model.OtherTypeDescription;
            WoodTypeDescription = model.WoodTypeDescription;
            ChemicalComposition = model.ChemicalComposition;
            Compositions = model.Compositions;
            AdditionalInfos = model.AdditionalInfos;

            SetAnnexMessageAndMergeDescriptions(annexNumber);
        }

        private void SetCompositions(WasteType wasteType)
        {
            var data = new List<ChemicalCompositionPercentages>();

            if (wasteType.WasteCompositions != null)
            {
                foreach (var c in wasteType.WasteCompositions)
                {
                    var ccp = new ChemicalCompositionPercentages();
                    ccp.Min = c.MinConcentration.ToString();
                    ccp.Max = c.MaxConcentration.ToString();
                    ccp.Name = c.Constituent + " wt/wt %";

                    if (c.MinConcentration > 0 && c.MaxConcentration > 0)
                    {
                        data.Add(ccp);
                    }
                }
            }

            Compositions = data;
        }

        private void SetAdditionalInfos(WasteType wasteType)
        {
            var data = new List<ChemicalCompositionPercentages>();

            if (wasteType.WasteAdditionalInformation != null)
            {
                foreach (var c in wasteType.WasteAdditionalInformation)
                {
                    var ccp = new ChemicalCompositionPercentages();
                    ccp.Min = c.MinConcentration.ToString();
                    ccp.Max = c.MaxConcentration.ToString();

                    if (c.WasteInformationType == WasteInformationType.HeavyMetals || c.WasteInformationType == WasteInformationType.NetCalorificValue)
                    {
                        ccp.Name = c.Constituent;
                    }
                    else
                    {
                        ccp.Name = c.Constituent + " wt/wt %";
                    }

                    if (c.MinConcentration > 0 && c.MaxConcentration > 0)
                    {
                        data.Add(ccp);
                    }
                }
            }

            AdditionalInfos = data;
        }

        private void SetAnnexMessageAndMergeDescriptions(int annexNumber)
        {
            var messageText = string.Empty;
            var seeAnnex = "See Annex " + annexNumber;

            if (HasAnnex || OptionalInformation.Length > 0)
            {
                messageText = seeAnnex;
            }

            if (ChemicalCompositionDescription.Length > textLength)
            {
                messageText = seeAnnex;
                LongDescription = ChemicalCompositionDescription;
            }
            else if (ChemicalCompositionDescription.Length > 0)
            {
                ShortDescription = ChemicalCompositionDescription;
            }

            if (OtherTypeDescription.Length > textLength)
            {
                messageText = seeAnnex;
                LongDescription = OtherTypeDescription;
            }
            else if (OtherTypeDescription.Length > 0)
            {
                ShortDescription = OtherTypeDescription;
            }

            if (WoodTypeDescription.Length > textLength)
            {
                messageText = seeAnnex;
                LongDescription = WoodTypeDescription;
            }
            else if (WoodTypeDescription.Length > 0)
            {
                ShortDescription = WoodTypeDescription;
            }

            AnnexMessage = messageText;
        }

        private void SetMergeDescriptionText()
        {
            if (ChemicalCompositionDescription.Length > textLength)
            {
                LongDescription = ChemicalCompositionDescription;
            }
            else if (ChemicalCompositionDescription.Length > 0)
            {
                ShortDescription = ChemicalCompositionDescription;
            }

            if (OtherTypeDescription.Length > textLength)
            {
                LongDescription = OtherTypeDescription;
            }
            else if (OtherTypeDescription.Length > 0)
            {
                ShortDescription = OtherTypeDescription;
            }

            if (WoodTypeDescription.Length > textLength)
            {
                LongDescription = WoodTypeDescription;
            }
            else if (WoodTypeDescription.Length > 0)
            {
                ShortDescription = WoodTypeDescription;
            }
        }

        private string GetWasteName(WasteType wasteType)
        {
            if (wasteType.ChemicalCompositionType == ChemicalComposition.RDF)
            {
                return "Refuse Derived Fuel (RDF)";
            }
            if (wasteType.ChemicalCompositionType == ChemicalComposition.SRF)
            {
                return "Solid Recovered Fuel (SRF)";
            }
            if (wasteType.ChemicalCompositionType == ChemicalComposition.Wood)
            {
                return "Wood";
            }
            if (wasteType.ChemicalCompositionType == ChemicalComposition.Other)
            {
                return wasteType.ChemicalCompositionName;
            }
            return string.Empty;
        }

        public static int TextLength()
        {
            return textLength;
        }
    }
}