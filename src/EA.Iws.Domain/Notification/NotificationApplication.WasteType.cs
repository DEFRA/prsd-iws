namespace EA.Iws.Domain.Notification
{
    using System;

    public partial class NotificationApplication
    {
        public void AddWasteType(ChemicalComposition chemicalCompositionType,
           string chemicalCompositionName, string chemicalCompositionDescription)
        {
            if (WasteType != null)
            {
                throw new InvalidOperationException(string.Format("Waste type already exists, cannot add another to notification: {0}", Id));
            }

            WasteType = new WasteType(chemicalCompositionType);
            WasteType.ChemicalCompositionDescription = chemicalCompositionDescription;
            WasteType.ChemicalCompositionName = chemicalCompositionName;
        }

        public void AddWasteComposition(string constituent, decimal minConcentration, decimal maxConcentration)
        {
            if (WasteType == null)
            {
                throw new InvalidOperationException(String.Format("Waste Type can not be null for notification: {0}", Id));
            }
            WasteComposition wasteComposition = new WasteComposition(constituent, minConcentration, maxConcentration);
            WasteType.AddWasteCompositions(wasteComposition);
        }
    }
}