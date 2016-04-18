namespace EA.Iws.RequestHandlers.Mappings.Reports
{
    using Core.Admin.Reports;
    using Domain.Reports;
    using Prsd.Core.Mapper;

    internal class ExportStatsMap : IMap<ExportStats, ExportStatsData>
    {
        public ExportStatsData Map(ExportStats source)
        {
            return new ExportStatsData
            {
                WasteCategory = source.WasteCategory,
                WasteStreams = source.WasteStreams,
                UNClass = GetValueOrDefault(source.UN),
                HCode = GetValueOrDefault(source.HCode),
                HCodeCharacteristics = GetValueOrDefault(source.HCodeDescription),
                AmountExported = source.QuantityReceived,
                CountriesOfTransit = source.TransitStates,
                CountryOfImport = source.CountryOfImport,
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