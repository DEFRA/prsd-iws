namespace EA.Iws.Web.Mappings.WasteCodes
{
    using System.Collections.Generic;
    using Areas.NotificationApplication.ViewModels.UNClass;
    using Areas.NotificationApplication.Views.Shared;
    using Core.WasteCodes;
    using Prsd.Core.Mapper;
    using Requests.WasteCodes;

    public class UNClassMap : WasteCodeMapBase, IMap<WasteCodeDataAndNotificationData, UNClassViewModel>
    {
        public UNClassMap(IMap<IEnumerable<WasteCodeData>, IList<WasteCodeViewModel>> mapper) : base(mapper)
        {
        }

        public UNClassViewModel Map(WasteCodeDataAndNotificationData source)
        {
            return new UNClassViewModel
            {
                EnterWasteCodesViewModel = MapCodes(source, CodeType.Un)
            };
        }
    }
}