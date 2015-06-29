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

        public void AddWasteType(WasteType wasteType)
        {
            if (WasteType != null)
            {
                throw new InvalidOperationException(string.Format("Waste type already exists, cannot add another to notification: {0}", Id));
            }

            WasteType = wasteType;
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
        public void AddWasteAdditionalInformation(IList<WasteAdditionalInformation> wasteType)
        {
            if (WasteType == null)
            {
                throw new InvalidOperationException(string.Format("Waste type does not exist on notification: {0}", Id));
            }
            WasteType.AddWasteAdditionalInformation(wasteType);
        }

        public void AddWoodTypeDescription(string woodTypeDescription)
        {
            if (WasteType == null)
            {
                throw new InvalidOperationException(String.Format("Waste Type can not be null for notification: {0}", Id));
            }
            WasteType.WoodTypeDescription = woodTypeDescription;
        }
        public void AddEnergyAndOptionalInformation(string energyInformation, string optionalInformation)
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