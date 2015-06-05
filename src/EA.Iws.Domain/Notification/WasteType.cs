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
    }
}