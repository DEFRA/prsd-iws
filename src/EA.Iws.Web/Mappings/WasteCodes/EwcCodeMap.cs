namespace EA.Iws.Web.Mappings.WasteCodes
{
    using System.Collections.Generic;
    using System.Linq;
    using Areas.NotificationApplication.ViewModels.EwcCode;
    using Areas.NotificationApplication.ViewModels.WasteCodes;
    using Areas.NotificationApplication.Views.Shared;
    using Core.WasteCodes;
    using Prsd.Core.Mapper;
    using Requests.WasteCodes;

    public class EwcCodeMap : IMap<WasteCodeDataAndNotificationData, EwcCodeViewModel>
    {
        private readonly IMap<IEnumerable<WasteCodeData>, IList<WasteCodeViewModel>> mapper;

        public EwcCodeMap(IMap<IEnumerable<WasteCodeData>, IList<WasteCodeViewModel>> mapper)
        {
            this.mapper = mapper;
        }

        public EwcCodeViewModel Map(WasteCodeDataAndNotificationData source)
        {
            WasteCodeData[] selectedCodes = null;

            if (source.NotificationWasteCodeData[CodeType.Ewc].Length > 0)
            {
                selectedCodes = source.NotificationWasteCodeData[CodeType.Basel];
            }

            var model = new EwcCodeViewModel
            {
                WasteCodesViewModel = new EnterWasteCodesViewModel
                {
                    SelectedWasteCodes = selectedCodes.Select(wc => wc.Id).ToList(),
                    IsNotApplicable = source.NotApplicableCodes.Any(nac => nac == CodeType.Ewc),
                    WasteCodes = mapper.Map(source.LookupWasteCodeData[CodeType.Ewc])
                }
            };

            return model;
        }
    }
}