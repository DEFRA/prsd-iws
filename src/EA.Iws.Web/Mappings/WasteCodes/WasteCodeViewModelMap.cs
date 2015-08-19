namespace EA.Iws.Web.Mappings.WasteCodes
{
    using System.Collections.Generic;
    using System.Linq;
    using Areas.NotificationApplication.Views.Shared;
    using Core.WasteCodes;
    using Prsd.Core.Mapper;

    public class WasteCodeViewModelMap : IMap<WasteCodeData, WasteCodeViewModel>,
        IMap<IEnumerable<WasteCodeData>, IList<WasteCodeViewModel>>
    {
        public WasteCodeViewModel Map(WasteCodeData source)
        {
            return new WasteCodeViewModel
            {
                Id = source.Id,
                Description = source.Description,
                Name = source.Code,
                CodeType = source.CodeType
            };
        }

        public IList<WasteCodeViewModel> Map(IEnumerable<WasteCodeData> source)
        {
            return source.Select(Map).ToList();
        }
    }
}