namespace EA.Iws.Web.Mappings.WasteCodes
{
    using System.Linq;
    using Areas.NotificationApplication.ViewModels.HCode;
    using Areas.NotificationApplication.ViewModels.WasteCodes;
    using Areas.NotificationApplication.Views.Shared;
    using Core.WasteCodes;
    using Prsd.Core.Mapper;
    using Requests.WasteCodes;

    public class HCodeMap : IMap<WasteCodeDataAndNotificationData, HCodeViewModel>
    {
        private readonly IMap<WasteCodeData, WasteCodeViewModel> wasteCodeMap;

        public HCodeMap(IMap<WasteCodeData, WasteCodeViewModel> wasteCodeMap)
        {
            this.wasteCodeMap = wasteCodeMap;
        }

        public HCodeViewModel Map(WasteCodeDataAndNotificationData source)
        {
            return new HCodeViewModel
            {
                EnterWasteCodesViewModel = new EnterWasteCodesViewModel
                {
                    WasteCodes = source.LookupWasteCodeData[CodeType.H].Select(wasteCodeMap.Map).ToList(),
                    SelectedWasteCodes = source.NotificationWasteCodeData[CodeType.H].Select(wc => wc.Id).ToList()
                }
            };
        }
    }
}