namespace EA.Iws.Web.Mappings.WasteCodes
{
    using System.Linq;
    using Areas.NotificationApplication.ViewModels.UNClass;
    using Areas.NotificationApplication.ViewModels.WasteCodes;
    using Areas.NotificationApplication.Views.Shared;
    using Core.WasteCodes;
    using Prsd.Core.Mapper;
    using Requests.WasteCodes;

    public class UNClassMap : IMap<WasteCodeDataAndNotificationData, UNClassViewModel>
    {
        private readonly IMap<WasteCodeData, WasteCodeViewModel> wasteCodeMap;

        public UNClassMap(IMap<WasteCodeData, WasteCodeViewModel> wasteCodeMap)
        {
            this.wasteCodeMap = wasteCodeMap;
        }

        public UNClassViewModel Map(WasteCodeDataAndNotificationData source)
        {
            return new UNClassViewModel
            {
                EnterWasteCodesViewModel = new EnterWasteCodesViewModel
                {
                    WasteCodes = source.LookupWasteCodeData[CodeType.Un].Select(wasteCodeMap.Map).ToList(),
                    SelectedWasteCodes = source.NotificationWasteCodeData[CodeType.Un].Select(wc => wc.Id).ToList()
                }
            };
        }
    }
}