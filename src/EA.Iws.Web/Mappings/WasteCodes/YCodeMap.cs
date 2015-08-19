namespace EA.Iws.Web.Mappings.WasteCodes
{
    using System.Linq;
    using Areas.NotificationApplication.ViewModels.WasteCodes;
    using Areas.NotificationApplication.Views.Shared;
    using Core.WasteCodes;
    using Prsd.Core.Mapper;
    using Requests.WasteCodes;

    public class YCodeMap : IMap<WasteCodeDataAndNotificationData, YCodesViewModel>
    {
        private readonly IMap<WasteCodeData, WasteCodeViewModel> wasteCodeMap;

        public YCodeMap(IMap<WasteCodeData, WasteCodeViewModel> wasteCodeMap)
        {
            this.wasteCodeMap = wasteCodeMap;
        }

        public YCodesViewModel Map(WasteCodeDataAndNotificationData source)
        {
            return new YCodesViewModel
            {
                EnterWasteCodesViewModel = new EnterWasteCodesViewModel
                {
                    WasteCodes = source.LookupWasteCodeData[CodeType.Y].Select(wasteCodeMap.Map).ToList(),
                    SelectedWasteCodes = source.NotificationWasteCodeData[CodeType.Y].Select(wc => wc.Id).ToList()
                }
            };
        }
    }
}