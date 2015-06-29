namespace EA.Iws.Requests.WasteType
{
    using System;
    using System.Collections.Generic;
    using Core.WasteType;
    using Prsd.Core.Mediator;

    public class UpdateWasteType : IRequest<Guid>
    {
        public UpdateWasteType(Guid notificationId, ChemicalCompositionType chemicalCompositionType,
            string furtherInformation, string energyInfo, List<WoodInformationData> wasteCompositions)
        {
            NotificationId = notificationId;
            ChemicalCompositionType = chemicalCompositionType;
            ChemicalCompositionDescription = FurtherInformation;
            WasteCompositions = wasteCompositions;
            EnergyInfo = energyInfo;
            FurtherInformation = furtherInformation;
        }

        public Guid NotificationId { get; private set; }

        public ChemicalCompositionType ChemicalCompositionType { get; private set; }

        public string ChemicalCompositionDescription { get; private set; }

        public string FurtherInformation { get; private set; }

        public string EnergyInfo { get; private set; }

        public List<WoodInformationData> WasteCompositions { get; private set; }
    }
}