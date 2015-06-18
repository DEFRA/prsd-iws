namespace EA.Iws.Domain.Notification
{
    using System;

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

        public void AddWasteGenerationProcess(string process, bool isDocumentAttached)
        {
            if (WasteType == null)
            {
                throw new InvalidOperationException(String.Format("Waste Type can not be null for notification: {0}", Id));
            }
            WasteType.WasteGenerationProcess = process;
            WasteType.IsDocumentAttached = isDocumentAttached;
        }

        public void AddPhysicalCharacteristic(PhysicalCharacteristicType physicalCharacteristic, string otherDescription = null)
        {
            if (WasteType == null)
            {
                throw new InvalidOperationException(String.Format("Waste Type can not be null for notification: {0}", Id));
            }
            WasteType.AddPhysicalCharacteristic(physicalCharacteristic, otherDescription);
        }

        public void AddWasteCode(WasteCode wasteCode)
        {
            if (WasteType == null)
            {
                throw new InvalidOperationException(String.Format("Waste Type can not be null for notification: {0}", Id));
            }
            WasteType.AddWasteCode(wasteCode);
        }

        public void AddBaselOrOecdWasteCode(WasteCode wasteCode)
        {
            if (WasteType == null)
            {
                throw new InvalidOperationException(String.Format("Waste Type can not be null for notification: {0}", Id));
            }
            WasteType.AddBaselOrOecdWasteCode(wasteCode);
        }
    }
}