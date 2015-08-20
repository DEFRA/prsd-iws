namespace EA.Iws.Web.Mappings.WasteCodes
{
    using System.Collections.Generic;
    using Areas.NotificationApplication.ViewModels.UNNumber;
    using Areas.NotificationApplication.Views.Shared;
    using Core.WasteCodes;
    using Prsd.Core.Mapper;
    using Requests.WasteCodes;

    public class UNNumberMap : WasteCodeMapBase, IMap<WasteCodeDataAndNotificationData, UNNumberViewModel>
    {
        public UNNumberMap(IMap<IEnumerable<WasteCodeData>, IList<WasteCodeViewModel>> mapper) : base(mapper)
        {
        }

        public UNNumberViewModel Map(WasteCodeDataAndNotificationData source)
        {
            return new UNNumberViewModel
            {
                EnterWasteCodesViewModel = MapCodes(source, CodeType.UnNumber)
            };
        }
    }
}