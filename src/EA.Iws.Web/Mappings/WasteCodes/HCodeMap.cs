namespace EA.Iws.Web.Mappings.WasteCodes
{
    using System.Collections.Generic;
    using Areas.NotificationApplication.ViewModels.HCode;
    using Areas.NotificationApplication.Views.Shared;
    using Core.WasteCodes;
    using Prsd.Core.Mapper;
    using Requests.WasteCodes;

    public class HCodeMap : WasteCodeMapBase, IMap<WasteCodeDataAndNotificationData, HCodeViewModel>
    {
        public HCodeMap(IMap<IEnumerable<WasteCodeData>, IList<WasteCodeViewModel>> mapper) : base(mapper)
        {
        }

        public HCodeViewModel Map(WasteCodeDataAndNotificationData source)
        {
            return new HCodeViewModel
            {
                EnterWasteCodesViewModel = MapCodes(source, CodeType.H)
            };
        }
    }
}