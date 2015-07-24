namespace EA.Iws.Domain.NotificationApplication
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

        private WasteType(ChemicalComposition chemicalComposition, string chemicalCompositionName)
        {
            ChemicalCompositionType = chemicalComposition;

            if (chemicalComposition == ChemicalComposition.Other)
            {
                Guard.ArgumentNotNull(() => chemicalCompositionName, chemicalCompositionName);
                ChemicalCompositionName = chemicalCompositionName;
            }
        }

        private WasteType(ChemicalComposition chemicalComposition,
            IList<WasteComposition> wasteCompositions, string chemicalCompositionDescription = "")
        {
            ChemicalCompositionType = chemicalComposition;

            if (!string.IsNullOrEmpty(chemicalCompositionDescription))
            {
                ChemicalCompositionDescription = chemicalCompositionDescription;
            }

            if (wasteCompositions == null)
            {
                WasteCompositionCollection = new List<WasteComposition>();
            }
            else
            {
                WasteCompositionCollection = new List<WasteComposition>(wasteCompositions);
            }
        }

        public ChemicalComposition ChemicalCompositionType { get; internal set; }

        private string chemicalCompositionDescription;
        public string ChemicalCompositionDescription
        {
            get
            {
                return chemicalCompositionDescription;
            }
            private set
            {
                if (ChemicalCompositionType == ChemicalComposition.Other || ChemicalCompositionType == ChemicalComposition.Wood)
                {
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
            internal set
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
            set { WasteCompositionCollection = value.ToList(); }
        }

        internal void SetWasteComposition(WasteComposition wasteComposition)
        {
            if (WasteCompositionCollection == null)
            {
                throw new InvalidOperationException("Waste Composition cannot be null");
            }
            WasteCompositionCollection.Add(wasteComposition);
        }

        internal void ClearWasteCompositions()
        {
            if (WasteCompositionCollection != null)
            {
                WasteCompositionCollection.Clear();
            }
        }

        internal void ClearWasteAdditionalInformation()
        {
            if (WasteAdditionalInformationCollection != null)
            {
                WasteAdditionalInformationCollection.Clear();
            }
        }

        private string otherWasteTypeDescription;
        public string OtherWasteTypeDescription
        {
            get
            {
                return otherWasteTypeDescription;
            }
            internal set
            {
                if (ChemicalCompositionType == ChemicalComposition.Other)
                {
                    otherWasteTypeDescription = value;
                }
                else if (!string.IsNullOrWhiteSpace(value))
                {
                    throw new InvalidOperationException("Other Waste Type description can only be set for chemical composition of other");
                }
            }
        }

        private string woodTypeDescription;
        public string WoodTypeDescription
        {
            get
            {
                return woodTypeDescription;
            }
            internal set
            {
                if (ChemicalCompositionType == ChemicalComposition.Wood)
                {
                    woodTypeDescription = value;
                }
                else if (!string.IsNullOrWhiteSpace(value))
                {
                    throw new InvalidOperationException("Wood type description can only be set for chemical composition of wood");
                }
            }
        }

        private string energyInformation;
        public string EnergyInformation
        {
            get
            {
                return energyInformation;
            }
            internal set
            {
                if (ChemicalCompositionType == ChemicalComposition.RDF || ChemicalCompositionType == ChemicalComposition.SRF)
                {
                    energyInformation = value;
                }
                else if (!string.IsNullOrWhiteSpace(value))
                {
                    throw new InvalidOperationException("energy information can only be set for chemical composition RDF or SRF");
                }
            }
        }

        private string optionalInformation;
        public string OptionalInformation
        {
            get
            {
                return optionalInformation;
            }
            internal set
            {
                if (ChemicalCompositionType == ChemicalComposition.RDF || ChemicalCompositionType == ChemicalComposition.SRF || ChemicalCompositionType == ChemicalComposition.Wood)
                {
                    optionalInformation = value;
                }
                else if (!string.IsNullOrWhiteSpace(value))
                {
                    throw new InvalidOperationException("optional information can only be set for chemical composition of Wood, RDF or SRF");
                }
            }
        }

        public bool HasAnnex { get; internal set; }

        protected virtual ICollection<WasteAdditionalInformation> WasteAdditionalInformationCollection { get; set; }

        public IEnumerable<WasteAdditionalInformation> WasteAdditionalInformation
        {
            get { return WasteAdditionalInformationCollection.ToSafeIEnumerable(); }
        }

        internal void SetWasteAdditionalInformation(IList<WasteAdditionalInformation> wasteAdditionalInformation)
        {
            WasteAdditionalInformationCollection = wasteAdditionalInformation;
        }

        public static WasteType CreateOtherWasteType(string chemicalCompositionName)
        {
            return new WasteType(ChemicalComposition.Other, chemicalCompositionName);
        }

        public static WasteType CreateWoodWasteType(string chemicalCompositionDescription, IList<WasteComposition> wasteCompositions)
        {
            return new WasteType(ChemicalComposition.Wood, wasteCompositions, chemicalCompositionDescription);
        }

        public static WasteType CreateRdfWasteType(IList<WasteComposition> wasteCompositions)
        {
            return new WasteType(ChemicalComposition.RDF, wasteCompositions);
        }

        public static WasteType CreateSrfWasteType(IList<WasteComposition> wasteCompositions)
        {
            return new WasteType(ChemicalComposition.SRF, wasteCompositions);
        }
    }
}