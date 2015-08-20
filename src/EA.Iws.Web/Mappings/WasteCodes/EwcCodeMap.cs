namespace EA.Iws.Web.Mappings.WasteCodes
{
    using System.Collections.Generic;
    using Areas.NotificationApplication.ViewModels.EwcCode;
    using Areas.NotificationApplication.Views.Shared;
    using Core.WasteCodes;
    using Prsd.Core.Mapper;
    using Requests.WasteCodes;

    public class EwcCodeMap : WasteCodeMapBase, IMap<WasteCodeDataAndNotificationData, EwcCodeViewModel>
    {
        public EwcCodeMap(IMap<IEnumerable<WasteCodeData>, IList<WasteCodeViewModel>> mapper) : base(mapper)
        {
        }

        public EwcCodeViewModel Map(WasteCodeDataAndNotificationData source)
        {
            return new EwcCodeViewModel
            {
                EnterWasteCodesViewModel = MapCodes(source, CodeType.Ewc)
            };
        }
    }
}