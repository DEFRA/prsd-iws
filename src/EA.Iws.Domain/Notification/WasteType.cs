namespace EA.Iws.Domain.Notification
{
    using System;
    using System.Collections.Generic;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using Prsd.Core.Extensions;

    public class WasteType : Entity
    {
        protected WasteType()
        {
        }

        internal WasteType(ChemicalComposition chemicalComposition)
        {
            ChemicalCompositionType = chemicalComposition;

            WasteCompositionCollection = new List<WasteComposition>();
            PhysicalCharacteristicsCollection = new List<PhysicalCharacteristicsInfo>();
        }

        protected virtual ICollection<PhysicalCharacteristicsInfo> PhysicalCharacteristicsCollection { get; set; }

        public IEnumerable<PhysicalCharacteristicsInfo> PhysicalCharacteristics
        {
            get { return PhysicalCharacteristicsCollection.ToSafeIEnumerable(); }
        }

        public ChemicalComposition ChemicalCompositionType { get; internal set; }

        private string chemicalCompositionDescription;
        public string ChemicalCompositionDescription
        {
            get
            {
                return chemicalCompositionDescription;
            }
            internal set
            {
                if (false == (ChemicalCompositionType == ChemicalComposition.RDF || ChemicalCompositionType == ChemicalComposition.SRF))
                {
                    Guard.ArgumentNotNull(value);
                    chemicalCompositionDescription = value;
                }
            }
        }

        private string chemicalCompositionName;
        public string ChemicalCompositionName
        {
            get
            {
                return chemicalCompositionName;
            }
            internal set
            {
                if (ChemicalCompositionType == ChemicalComposition.Other)
                {
                    Guard.ArgumentNotNull(value);
                    chemicalCompositionName = value;
                }
            }
        }

        protected virtual ICollection<WasteComposition> WasteCompositionCollection { get; set; }

        public IEnumerable<WasteComposition> WasteCompositions
        {
            get { return WasteCompositionCollection.ToSafeIEnumerable(); }
        }

        public void AddWasteCompositions(WasteComposition wasteComposition)
        {
            if (WasteCompositionCollection == null)
            {
                throw new InvalidOperationException("Waste Composition can not be null");
            }
            WasteCompositionCollection.Add(wasteComposition);
        }

        public void AddPhysicalCharacteristic(PhysicalCharacteristicType physicalCharacteristic, string otherDescription = null)
        {
            if (PhysicalCharacteristicsCollection == null)
            {
                throw new InvalidOperationException("Physical characteristics collection can not be null");
            }

            var physicalCharacteristicInfo = new PhysicalCharacteristicsInfo(physicalCharacteristic);
            if (!string.IsNullOrEmpty(otherDescription) && physicalCharacteristic == PhysicalCharacteristicType.Other)
            {
                physicalCharacteristicInfo.OtherDescription = otherDescription;
            }
            PhysicalCharacteristicsCollection.Add(physicalCharacteristicInfo);
        }

        public string WasteGenerationProcess { get; internal set; }

        public bool IsDocumentAttached { get; internal set; }
    }
}