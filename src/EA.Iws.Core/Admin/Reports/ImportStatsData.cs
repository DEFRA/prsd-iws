namespace EA.Iws.Core.Admin.Reports
{
    using System.ComponentModel;

    public class ImportStatsData
    {
        [DisplayName("Waste Category (Y Code)")]
        public string WasteCategory { get; set; }

        public string WasteStreams { get; set; }

        [DisplayName("Annex VIII")]
        public string AnnexViii { get; set; }

        [DisplayName("UN Class")]
        public string UNClass { get; set; }

        [DisplayName("H Code")]
        public string HCode { get; set; }

        [DisplayName("H Code Characteristics")]
        public string HCodeCharacteristics { get; set; }

        public decimal AmountImported { get; set; }

        public string CountriesOfTransit { get; set; }

        public string CountryOfExport { get; set; }

        [DisplayName("D Code")]
        public string DCode { get; set; }

        [DisplayName("R Code")]
        public string RCode { get; set; }

        [DisplayName("EWC Codes")]
        public string EwcCodes { get; set; }
    }
}