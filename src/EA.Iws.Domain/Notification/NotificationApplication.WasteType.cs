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

        public void AddWasteType(ChemicalComposition chemicalCompositionType,
           string chemicalCompositionName, string chemicalCompositionDescription, 
            List<WasteComposition> wasteCompositions)
        {
            if (WasteType != null)
            {
                throw new InvalidOperationException(string.Format("Waste type already exists, cannot add another to notification: {0}", Id));
            }

            WasteType = new WasteType(chemicalCompositionType);
            WasteType.ChemicalCompositionDescription = chemicalCompositionDescription;
            WasteType.ChemicalCompositionName = chemicalCompositionName;
            
            if (chemicalCompositionType == ChemicalComposition.RDF ||
                chemicalCompositionType == ChemicalComposition.SRF)
            {
                if (wasteCompositions == null || wasteCompositions.Count == 0)
                {
                    throw new ArgumentException(string.Format("Waste composition is required when waste type is either RDF or SRF for notification: {0}", Id));
                }

                foreach (var wasteComposition in wasteCompositions)
                {
                    WasteType.AddWasteComposition(wasteComposition);
                }
            }
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
    }
}