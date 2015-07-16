namespace EA.Iws.Domain.Notification
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
            if (WasteType != null && WasteType.ChemicalCompositionType != wasteType.ChemicalCompositionType)
            {
                ClearExistingWasteCompositionInformation();
            }

            if (WasteType != null && wasteType.ChemicalCompositionType != ChemicalComposition.Other)
            {
                if (wasteType.ChemicalCompositionType != WasteType.ChemicalCompositionType)
                {
                    WasteType.ClearWasteAdditionalInformation();
                    WasteType.ClearWasteCompositions();
                    WasteType = wasteType;
                }
                else
                {
                    WasteType.ChemicalCompositionType = wasteType.ChemicalCompositionType;
                    WasteType.WasteCompositions = wasteType.WasteCompositions;
                }
            }
            else if (WasteType != null && wasteType.ChemicalCompositionType == ChemicalComposition.Other)
            {
                WasteType.ClearWasteAdditionalInformation();
                ClearExistingWasteCompositionInformation();
                WasteType.ChemicalCompositionType = wasteType.ChemicalCompositionType;
                WasteType.ChemicalCompositionName = wasteType.ChemicalCompositionName;
            }
            else
            {
                WasteType = wasteType; 
            }
        }

        private void ClearExistingWasteCompositionInformation()
        {
            if (WasteType != null && WasteType.WasteCompositions != null)
            {
                WasteType.OptionalInformation = null;
                WasteType.EnergyInformation = null;
                WasteType.WoodTypeDescription = null;
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

        public void SetEnergyAndOptionalInformation(string energyInformation, string optionalInformation)
        {
            if (WasteType == null)
            {
                throw new InvalidOperationException(String.Format("Waste Type can not be null for notification: {0}", Id));
            }
            WasteType.EnergyInformation = energyInformation;
            WasteType.OptionalInformation = optionalInformation;
        }
    }
}