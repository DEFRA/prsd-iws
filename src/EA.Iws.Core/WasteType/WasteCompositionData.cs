namespace EA.Iws.Core.WasteType
{
    using System;

    public class WasteCompositionData
    {
        public Guid Id { get; set; }

        public Guid WasteTypeId { get; set; }

        public string Constituent { get; set; }

        public decimal? MinConcentration { get; set; }

        public decimal? MaxConcentration { get; set; }

        public ChemicalCompositionCategory ChemicalCompositionCategory { get; set; }
    }
}
