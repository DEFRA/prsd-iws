namespace EA.Iws.Web.Mappings.WasteCodes
{
    using System.Collections.Generic;
    using Areas.NotificationApplication.ViewModels.WasteCodes;
    using Areas.NotificationApplication.Views.Shared;
    using Core.WasteCodes;
    using Prsd.Core.Mapper;
    using Requests.WasteCodes;

    public class YCodeMap : WasteCodeMapBase, IMap<WasteCodeDataAndNotificationData, YCodeViewModel>
    {
        public YCodeMap(IMap<IEnumerable<WasteCodeData>, IList<WasteCodeViewModel>> mapper) : base(mapper)
        {
        }

        public YCodeViewModel Map(WasteCodeDataAndNotificationData source)
        {
            return new YCodeViewModel
            {
                EnterWasteCodesViewModel = MapCodes(source, CodeType.Y)
            };
        }
    }
}