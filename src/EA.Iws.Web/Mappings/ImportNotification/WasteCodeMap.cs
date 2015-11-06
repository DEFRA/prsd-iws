namespace EA.Iws.Web.Mappings.ImportNotification
{
    using System.Collections.Generic;
    using System.Linq;
    using Areas.ImportNotification.ViewModels.Shared;
    using Core.WasteCodes;
    using Prsd.Core.Mapper;

    public class WasteCodeMap : IMap<WasteCodeData, WasteCodeViewModel>, IMap<List<WasteCodeData>, List<WasteCodeViewModel>>
    {
        public List<WasteCodeViewModel> Map(List<WasteCodeData> source)
        {
            return source.Select(Map).ToList();
        }

        public WasteCodeViewModel Map(WasteCodeData source)
        {
            return new WasteCodeViewModel
            {
                Id = source.Id,
                Name = source.Code,
                CodeType = source.CodeType
            };
        }
    }
}