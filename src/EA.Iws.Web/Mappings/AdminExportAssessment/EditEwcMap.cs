namespace EA.Iws.Web.Mappings.AdminExportAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Areas.AdminExportAssessment.ViewModels.EditEwcCode;
    using Areas.NotificationApplication.Views.Shared;
    using Core.WasteCodes;
    using Prsd.Core.Mapper;
    using Requests.WasteCodes;

    public class EditEwcMap : IMap<WasteCodeDataAndNotificationData, EditEwcCodeViewModel>
    {
        private readonly IMap<IEnumerable<WasteCodeData>, IList<WasteCodeViewModel>> mapper;

        public EditEwcMap(IMap<IEnumerable<WasteCodeData>, IList<WasteCodeViewModel>> mapper)
        {
            this.mapper = mapper;
        }

        public EditEwcCodeViewModel Map(WasteCodeDataAndNotificationData source)
        {
            return new EditEwcCodeViewModel
            {
                EnterWasteCodesViewModel = new EditEwcEnterCodesViewModel
                {
                    SelectedWasteCodes = source.NotificationWasteCodeData[CodeType.Ewc]
                    .Where(wc => wc.Id != Guid.Empty).Select(wc => wc.Id).ToList(),
                    WasteCodes = mapper.Map(source.LookupWasteCodeData[CodeType.Ewc])
                }
            };
        }
    }
}