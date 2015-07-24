namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using System.Collections.Generic;

    public partial class NotificationApplication
    {
        public bool HasWasteType
        {
            get { return WasteType != null; }
        }

        public void SetWasteType(WasteType wasteType)
        {
            if (WasteType == null)
            {
                WasteType = wasteType;
                return;
            }

            if (WasteType.ChemicalCompositionType == wasteType.ChemicalCompositionType)
            {
                UpdateSameChemicalCompositionType(wasteType);
            }
            else 
            {
                ClearAllWasteData();
                WasteType = wasteType;
            }
        }

        private void UpdateSameChemicalCompositionType(WasteType wasteType)
        {
            if (wasteType.ChemicalCompositionType == ChemicalComposition.Other)
            {
                WasteType.ChemicalCompositionName = wasteType.ChemicalCompositionName;
            }

            if (wasteType.ChemicalCompositionType == ChemicalComposition.RDF || wasteType.ChemicalCompositionType == ChemicalComposition.SRF)
            {
                ClearWasteCompositions();
                WasteType.WasteCompositions = wasteType.WasteCompositions;
            }

            if (wasteType.ChemicalCompositionType == ChemicalComposition.Wood)
            {
                ClearWasteCompositions();
                WasteType.WasteCompositions = wasteType.WasteCompositions;
                WasteType.WoodTypeDescription = wasteType.WoodTypeDescription;
            }
        }

        private void ClearAllWasteData()
        {
            if (WasteType != null)
            {
                WasteType.OptionalInformation = null;
                WasteType.EnergyInformation = null;
                WasteType.WoodTypeDescription = null;
                WasteType.OtherWasteTypeDescription = null;
                WasteType.HasAnnex = false;
                ClearWasteCompositions();
                ClearWasteAdditionalInformation();
            }
        }

        private void ClearWasteCompositions()
        {
            if (WasteType != null && WasteType.WasteCompositions != null)
            {
                WasteType.ClearWasteCompositions();
            }
        }

        private void ClearWasteAdditionalInformation()
        {
            if (WasteType != null && WasteType.WasteAdditionalInformation != null)
            {
                WasteType.ClearWasteAdditionalInformation();
            }
        }

        public void AddOtherWasteTypeAdditionalInformation(string description, bool hasAnnex)
        {
            if (WasteType == null)
            {
                throw new InvalidOperationException(string.Format("Waste type does not exist on notification: {0}", Id));
            }
            WasteType.OtherWasteTypeDescription = description;
            WasteType.HasAnnex = hasAnnex;
        }

        public void SetWasteAdditionalInformation(IList<WasteAdditionalInformation> wasteType)
        {
            if (WasteType == null)
            {
                throw new InvalidOperationException(string.Format("Waste type does not exist on notification: {0}", Id));
            }
            ClearWasteAdditionalInformation();
            WasteType.SetWasteAdditionalInformation(wasteType);
        }

        public void SetWoodTypeDescription(string woodTypeDescription)
        {
            if (WasteType == null)
            {
                throw new InvalidOperationException(String.Format("Waste Type can not be null for notification: {0}", Id));
            }
            WasteType.WoodTypeDescription = woodTypeDescription;
        }

        public void SetEnergyAndOptionalInformation(string energyInformation, string optionalInformation, bool hasAnnex)
        {
            if (WasteType == null)
            {
                throw new InvalidOperationException(String.Format("Waste Type can not be null for notification: {0}", Id));
            }
            WasteType.EnergyInformation = energyInformation;
            WasteType.OptionalInformation = optionalInformation;
            WasteType.HasAnnex = hasAnnex;
        }
    }
}