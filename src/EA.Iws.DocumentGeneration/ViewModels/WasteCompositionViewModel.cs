namespace EA.Iws.DocumentGeneration.ViewModels
{
    using System.Collections.Generic;
    using Domain.NotificationApplication;
    using Formatters;

    internal class WasteCompositionViewModel
    {
        private const int textLength = 150;

        private string longDescription = string.Empty;
        private string shortDescription = string.Empty;
        private string annexMessage = string.Empty;

        private string otherTypeDescription = string.Empty;
        private string woodTypeDescription = string.Empty;
        private string chemicalCompositionDescription = string.Empty;

        public WasteCompositionViewModel(WasteType wasteType, WasteCompositionFormatter formatter)
        {
            WasteName = formatter.GetWasteName(wasteType);
            Energy = formatter.GetEnergyEfficiencyString(wasteType);
            OptionalInformation = formatter.GetOptionalInformationTitle(wasteType);

            HasAnnex = wasteType.HasAnnex;

            SetWasteTypeDescription(wasteType);

            ChemicalComposition = wasteType.ChemicalCompositionType;
            Compositions = formatter.GetWasteCompositionPercentages(wasteType);
            AdditionalInfos =
                formatter.GetAdditionalInformationChemicalCompositionPercentages(wasteType.WasteAdditionalInformation);

            SetMergeDescriptionText();
        }

        private void SetWasteTypeDescription(WasteType wasteType)
        {
            if (wasteType == null)
            {
                return;
            }

            if (wasteType.ChemicalCompositionType == ChemicalComposition.Wood)
            {
                WoodTypeDescription = wasteType.WoodTypeDescription ?? string.Empty;
            }
            else if (wasteType.ChemicalCompositionType == ChemicalComposition.Other)
            {
                OtherTypeDescription = wasteType.OtherWasteTypeDescription ?? string.Empty;
            }
            else
            {
                ChemicalCompositionDescription = wasteType.ChemicalCompositionDescription ?? string.Empty;
            }
        }

        public WasteCompositionViewModel(WasteCompositionViewModel model, int annexNumber)
        {
            WasteName = model.WasteName;
            HasAnnex = model.HasAnnex;
            chemicalCompositionDescription = model.ChemicalCompositionDescription;
            ShortDescription = model.ShortDescription;
            LongDescription = model.LongDescription;
            Energy = model.Energy;
            OptionalInformation = model.OptionalInformation;
            otherTypeDescription = model.OtherTypeDescription;
            woodTypeDescription = model.WoodTypeDescription;
            ChemicalComposition = model.ChemicalComposition;
            Compositions = model.Compositions;
            AdditionalInfos = model.AdditionalInfos;

            SetAnnexMessageAndMergeDescriptions(annexNumber);
        }

        public string WasteName { get; private set; }

        public bool HasAnnex { get; private set; }

        public string ChemicalCompositionDescription
        {
            get { return chemicalCompositionDescription; }
            private set { chemicalCompositionDescription = value; }
        }

        public string OtherTypeDescription
        {
            get { return otherTypeDescription; }
            private set { otherTypeDescription = value; }
        }

        public string WoodTypeDescription
        {
            get { return woodTypeDescription; }
            private set { woodTypeDescription = value; }
        }

        public string ShortDescription
        {
            get { return shortDescription; }
            private set { shortDescription = value; }
        }

        public string LongDescription
        {
            get { return longDescription; }
            private set { longDescription = value; }
        }

        public string Energy { get; private set; }

        public string OptionalInformation { get; private set; }

        public ChemicalComposition ChemicalComposition { get; private set; }

        public IList<ChemicalCompositionPercentages> Compositions { get; private set; }

        public IList<ChemicalCompositionPercentages> AdditionalInfos { get; private set; }

        public string AnnexMessage
        {
            get { return annexMessage; }
            private set { annexMessage = value; }
        }

        private bool ChemicalCompositionDescriptionOverflowsToAnnex()
        {
            return ChemicalCompositionDescription.Length > textLength;
        }

        private bool HasChemicalCompositionDescription()
        {
            return !string.IsNullOrWhiteSpace(ChemicalCompositionDescription);
        }

        protected void SetAnnexMessageAndMergeDescriptions(int annexNumber)
        {
            var messageText = string.Empty;
            var seeAnnex = "See Annex " + annexNumber;

            if (HasAnnex || OptionalInformation.Length > 0)
            {
                messageText = seeAnnex;
            }

            if (ChemicalCompositionDescriptionOverflowsToAnnex())
            {
                messageText = seeAnnex;
                LongDescription = ChemicalCompositionDescription;
            }
            else if (HasChemicalCompositionDescription())
            {
                messageText = seeAnnex;
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
                messageText = seeAnnex;
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

        public static int TextLength()
        {
            return textLength;
        }
    }
}