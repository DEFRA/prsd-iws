namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.WasteCodes;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;

    internal class WasteCodeMap : IMap<WasteCode, WasteCodeData>,
        IMap<IEnumerable<WasteCodeInfo>, WasteCodeData[]>,
        IMap<IEnumerable<WasteCode>, WasteCodeData[]>,
        IMap<WasteCodeInfo, WasteCodeData>,
        IMapWithParameter<NotificationApplication, CodeType, WasteCodeData[]>
    {
        public WasteCodeData Map(WasteCode source)
        {
            return new WasteCodeData
            {
                Id = source.Id,
                Description = source.Description,
                Code = source.Code,
                CodeType = source.CodeType,
            };
        }

        public WasteCodeData[] Map(IEnumerable<WasteCodeInfo> source)
        {
            return source.Select(Map).ToArray();
        }

        public WasteCodeData Map(WasteCodeInfo source)
        {
            if (source == null)
            {
                return new WasteCodeData();
            }

            if (HasAWasteCode(source))
            {
                 return Map(source.WasteCode);
            }
            
            if (source.IsNotApplicable)
            {
                return new WasteCodeData
                {
                    Id = Guid.Empty,
                    CodeType = source.CodeType,
                    IsNotApplicable = true
                };
            }

            if (IsACustomCode(source))
            {
                return
                    new WasteCodeData
                    {
                        Id = Guid.Empty,
                        CodeType = source.CodeType,
                        Code = source.CustomCode,
                        CustomCode = source.CustomCode
                    };
            }

            return new WasteCodeData();
        }

        private bool HasAWasteCode(WasteCodeInfo source)
        {
            return source.WasteCode != null;
        }

        private bool IsACustomCode(WasteCodeInfo source)
        {
            return !string.IsNullOrWhiteSpace(source.CustomCode);
        }

        public WasteCodeData[] Map(NotificationApplication source, CodeType parameter)
        {
            switch (parameter)
            {
                case CodeType.Ewc:
                    return Map(source.EwcCodes);
                case CodeType.Basel:
                case CodeType.Oecd:
                    return (source.BaselOecdCode == null) ? null : new[] { Map(source.BaselOecdCode) };
                case CodeType.ExportCode:
                    return (source.ExportCode == null) ? null : new[] { Map(source.ExportCode) };
                case CodeType.ImportCode:
                    return (source.ImportCode == null) ? null : new[] { Map(source.ImportCode) };
                case CodeType.OtherCode:
                    return (source.OtherCode == null) ? null : new[] { Map(source.OtherCode) };
                case CodeType.Y:
                    return Map(source.YCodes);
                case CodeType.H:
                    return Map(source.HCodes);
                case CodeType.Un:
                    return Map(source.UnClasses);
                case CodeType.UnNumber:
                    return Map(source.UnNumbers);
                case CodeType.CustomsCode:
                    return (source.CustomsCode == null) ? null : new[] { Map(source.CustomsCode) };
                default:
                    throw new InvalidOperationException(string.Format("Unknown code type {0}", parameter));
            }
        }

        public WasteCodeData[] Map(IEnumerable<WasteCode> source)
        {
            return source.Select(wc => Map(wc)).ToArray();
        }
    }
}