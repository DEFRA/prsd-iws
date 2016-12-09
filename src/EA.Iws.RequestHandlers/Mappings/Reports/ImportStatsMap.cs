namespace EA.Iws.RequestHandlers.Mappings.Reports
{
    using Core.Admin.Reports;
    using Domain.Reports;
    using Prsd.Core.Mapper;

    internal class ImportStatsMap : IMap<ImportStats, ImportStatsData>
    {
        public ImportStatsData Map(ImportStats source)
        {
            return new ImportStatsData
            {
                WasteCategory = source.WasteCategory,
                WasteStreams = source.WasteStreams,
                UNClass = GetValueOrDefault(source.UN),
                HCode = GetValueOrDefault(source.HCode),
                HCodeCharacteristics = GetValueOrDefault(source.HCodeDescription),
                AmountImported = source.QuantityReceived,
                CountriesOfTransit = source.TransitStates,
                CountryOfExport = source.CountryOfExport,
                DCode = source.DCode,
                RCode = source.RCode,
                EwcCodes = source.Ewc
            };
        }

        private static string GetValueOrDefault(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? "NA" : value;
        }
    }
}