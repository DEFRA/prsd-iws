namespace EA.Iws.Domain.Notification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using Prsd.Core.Extensions;

    public class WasteType : Entity
    {
        protected WasteType()
        {
        }

        private WasteType(ChemicalComposition chemicalComposition, string chemicalCompositionName,
            string chemicalCompositionDescription)
        {
            ChemicalCompositionType = chemicalComposition;

            Guard.ArgumentNotNull(() => chemicalCompositionDescription, chemicalCompositionDescription);

            if (chemicalComposition == ChemicalComposition.Other)
            {
                Guard.ArgumentNotNull(() => chemicalCompositionName, chemicalCompositionName);
                ChemicalCompositionName = chemicalCompositionName;
            }

            ChemicalCompositionDescription = chemicalCompositionDescription;

            PhysicalCharacteristicsCollection = new List<PhysicalCharacteristicsInfo>();

            WasteCodeInfoCollection = new List<WasteCodeInfo>();
        }

        private WasteType(ChemicalComposition chemicalComposition,
            IList<WasteComposition> wasteCompositions)
        {
            ChemicalCompositionType = chemicalComposition;

            if (chemicalComposition == ChemicalComposition.RDF ||
                chemicalComposition == ChemicalComposition.SRF)
            {
                if (wasteCompositions == null || wasteCompositions.Count == 0)
                {
                    throw new ArgumentException("Waste composition is required when waste type is either RDF or SRF", "wasteCompositions");
                }
            }

            if (wasteCompositions == null)
            {
                WasteCompositionCollection = new List<WasteComposition>();
            }
            else
            {
                WasteCompositionCollection = new List<WasteComposition>(wasteCompositions);
            }

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
            protected set
            {
                if (ChemicalCompositionType == ChemicalComposition.Other || ChemicalCompositionType == ChemicalComposition.Wood)
                {
                    Guard.ArgumentNotNull(() => value, value);
                    chemicalCompositionDescription = value;
                }
                else if (!string.IsNullOrWhiteSpace(value))
                {
                    throw new InvalidOperationException("Chemical composition description can only be set for chemical composition of wood or other.");
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
            protected set
            {
                if (ChemicalCompositionType == ChemicalComposition.Other)
                {
                    Guard.ArgumentNotNull(() => value, value);
                    chemicalCompositionName = value;
                }
                else if (!string.IsNullOrWhiteSpace(value))
                {
                    throw new InvalidOperationException("Chemical composition name can only be set for chemical composition of other");
                }
            }
        }

        protected virtual ICollection<WasteComposition> WasteCompositionCollection { get; set; }

        public IEnumerable<WasteComposition> WasteCompositions
        {
            get { return WasteCompositionCollection.ToSafeIEnumerable(); }
        }

        internal void AddWasteComposition(WasteComposition wasteComposition)
        {
            if (WasteCompositionCollection == null)
            {
                throw new InvalidOperationException("Waste Composition cannot be null");
            }
            WasteCompositionCollection.Add(wasteComposition);
        }

        internal void AddPhysicalCharacteristic(PhysicalCharacteristicType physicalCharacteristic, string otherDescription = null)
        {
            if (PhysicalCharacteristicsCollection == null)
            {
                throw new InvalidOperationException("Physical characteristics collection cannot be null");
            }

            var physicalCharacteristicInfo = new PhysicalCharacteristicsInfo(physicalCharacteristic);
            if (!string.IsNullOrEmpty(otherDescription))
            {
                if (physicalCharacteristic != PhysicalCharacteristicType.Other)
                {
                    throw new InvalidOperationException(string.Format("Other description cannot be set when the physical characteristic type is not other for waste type {0}", Id));
                }
                physicalCharacteristicInfo.OtherDescription = otherDescription;
            }
            PhysicalCharacteristicsCollection.Add(physicalCharacteristicInfo);
        }

        protected virtual ICollection<WasteCodeInfo> WasteCodeInfoCollection { get; set; }

        public IEnumerable<WasteCodeInfo> WasteCodeInfo
        {
            get { return WasteCodeInfoCollection.ToSafeIEnumerable(); }
        }

        public string WasteGenerationProcess { get; internal set; }

        public bool IsDocumentAttached { get; internal set; }

        public static WasteType CreateOtherWasteType(string chemicalCompositionName, string chemicalCompositionDescription)
        {
            return new WasteType(ChemicalComposition.Other, chemicalCompositionName, chemicalCompositionDescription);
        }

        public static WasteType CreateWoodWasteType(string chemicalCompositionDescription)
        {
            return new WasteType(ChemicalComposition.Wood, null, chemicalCompositionDescription);
        }

        public static WasteType CreateRdfWasteType(IList<WasteComposition> wasteCompositions)
        {
            return new WasteType(ChemicalComposition.RDF, wasteCompositions);
        }

        public static WasteType CreateSrfWasteType(IList<WasteComposition> wasteCompositions)
        {
            return new WasteType(ChemicalComposition.SRF, wasteCompositions);
        }

        public void AddWasteCode(WasteCode wasteCode)
        {
            if (WasteCodeInfoCollection == null)
            {
                throw new InvalidOperationException(string.Format("WasteCodeInfoCollection cannot be null for notification {0}", Id));
            }

            var wasteCodeInfo = new WasteCodeInfo(wasteCode);
            WasteCodeInfoCollection.Add(wasteCodeInfo);
        }

        public void AddBaselOrOecdWasteCode(WasteCode wasteCode)
        {
            if (WasteCodeInfoCollection == null)
            {
                throw new InvalidOperationException(string.Format("WasteCodeInfoCollection cannot be null for notification {0}", Id));
            }

            if (wasteCode.CodeType == CodeType.Basel && WasteCodeInfoCollection.Any(c => c.WasteCode.CodeType == CodeType.Basel))
            {
                throw new InvalidOperationException(string.Format("A Basel code already exists for notification {0}", Id));
            }

            if (wasteCode.CodeType == CodeType.Oecd && WasteCodeInfoCollection.Any(c => c.WasteCode.CodeType == CodeType.Oecd))
            {
                throw new InvalidOperationException(string.Format("A Oecd code already exists for notification {0}", Id));
            }

            if (WasteCodeInfoCollection.Any(c => c.WasteCode.Code == wasteCode.Code))
            {
                throw new InvalidOperationException(string.Format("The same code cannot be entered twice for notification {0}", Id));
            }

            var wasteCodeInfo = new WasteCodeInfo(wasteCode);
            WasteCodeInfoCollection.Add(wasteCodeInfo);
        }
    }
}