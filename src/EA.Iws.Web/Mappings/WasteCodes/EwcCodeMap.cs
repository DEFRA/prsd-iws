namespace EA.Iws.Web.Mappings.WasteCodes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Areas.NotificationApplication.ViewModels.EwcCode;
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
            return new EwcCodeViewModel
            {
                EnterWasteCodesViewModel = new EwcEnterCodesViewModel
                {
                    SelectedWasteCodes = source.NotificationWasteCodeData[CodeType.Ewc]
                    .Where(wc => wc.Id != Guid.Empty).Select(wc => wc.Id).ToList(),
                    WasteCodes = mapper.Map(source.LookupWasteCodeData[CodeType.Ewc])
                }
            };
        }
    }
}