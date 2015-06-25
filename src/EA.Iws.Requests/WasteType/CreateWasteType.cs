namespace EA.Iws.Requests.WasteType
{
    using System;
    using System.Collections.Generic;
    using Core.WasteType;
    using Prsd.Core.Mediator;

    public class CreateWasteType : IRequest<Guid>
    {
        public CreateWasteType(Guid notificationId, ChemicalCompositionType chemicalCompositionType,
            string chemicalCompositionName, string chemicalCompositionDescription,
            List<WasteCompositionData> wasteCompositions)
        {
            NotificationId = notificationId;
            ChemicalCompositionType = chemicalCompositionType;
            ChemicalCompositionName = chemicalCompositionName;
            ChemicalCompositionDescription = chemicalCompositionDescription;
            WasteCompositions = wasteCompositions;
        }

        public Guid NotificationId { get; private set; }

        public ChemicalCompositionType ChemicalCompositionType { get; private set; }

        public string ChemicalCompositionName { get; private set; }

        public string ChemicalCompositionDescription { get; private set; }

        public List<WasteCompositionData> WasteCompositions { get; private set; }
    }
}
