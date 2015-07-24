namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using System.Collections.Generic;
    using Core.WasteCodes;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;

    internal class WasteCodeMap : IMap<WasteCode, WasteCodeData>, IMap<IEnumerable<WasteCodeInfo>, WasteCodeData[]>,
        IMap<WasteCodeInfo, WasteCodeData[]>, IMapWithParameter<NotificationApplication, CodeType, WasteCodeData[]>
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

        public WasteCodeData[] Map(NotificationApplication source, CodeType parameter)
        {
            switch (parameter)
            {
                case CodeType.Ewc:
                    return Map(source.EwcCodes);
                case CodeType.Basel:
                case CodeType.Oecd:
                    return Map(source.BaselOecdCode);
                case CodeType.ExportCode:
                    return Map(source.ExportCode);
                case CodeType.ImportCode:
                    return Map(source.ImportCode);
                case CodeType.OtherCode:
                    return Map(source.OtherCode);
                case CodeType.Y:
                    return Map(source.YCodes);
                case CodeType.H:
                    return Map(source.HCodes);
                case CodeType.Un:
                    return Map(source.UnClasses);
                case CodeType.UnNumber:
                    return Map(source.UnNumbers);
                case CodeType.CustomsCode:
                    return Map(source.CustomsCodes);
                default:
                    throw new InvalidOperationException(string.Format("Unknown code type {0}", parameter));
            }
        }
    }
}