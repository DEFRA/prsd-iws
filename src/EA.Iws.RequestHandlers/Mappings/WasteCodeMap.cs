namespace EA.Iws.RequestHandlers.Mappings
{
    using System.Collections.Generic;
    using Core.WasteCodes;
    using Domain.Notification;
    using Prsd.Core.Mapper;

    internal class WasteCodeMap : IMap<WasteCode, WasteCodeData>, IMap<IEnumerable<WasteCodeInfo>, WasteCodeData[]>,
        IMap<WasteCodeInfo, WasteCodeData[]>
    {
        public WasteCodeData Map(WasteCode source)
        {
            return new WasteCodeData
            {
                Id = source.Id,
                Description = source.Description,
                Code = source.Code,
                CodeType = source.CodeType
            };
        }

        public WasteCodeData[] Map(IEnumerable<WasteCodeInfo> source)
        {
            var wasteCodes = new List<WasteCodeData>();
            foreach (var wasteCode in source)
            {
                var wasteCodeData = Map(wasteCode.WasteCode);
                wasteCodeData.CustomCode = wasteCode.CustomCode;
                wasteCodes.Add(wasteCodeData);
            }
            return wasteCodes.ToArray();
        }

        public WasteCodeData[] Map(WasteCodeInfo source)
        {
            if (source == null)
            {
                return new WasteCodeData[] { };
            }

            var wasteCodeData = Map(source.WasteCode);
            wasteCodeData.CustomCode = source.CustomCode;
            return new[]
            {
                wasteCodeData
            };
        }
    }
}