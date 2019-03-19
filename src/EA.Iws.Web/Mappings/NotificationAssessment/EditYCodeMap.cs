namespace EA.Iws.Web.Mappings.NotificationAssessment
{
    using System.Collections.Generic;
    using Areas.AdminExportAssessment.ViewModels.YCode;
    using Areas.NotificationApplication.Views.Shared;
    using Core.WasteCodes;
    using Prsd.Core.Mapper;
    using Requests.WasteCodes;
    using WasteCodes;

    public class EditYCodeMap : WasteCodeMapBase, IMap<WasteCodeDataAndNotificationData, EditYCodeViewModel>
    {
        public EditYCodeMap(IMap<IEnumerable<WasteCodeData>, IList<WasteCodeViewModel>> mapper) : base(mapper)
        {
        }

        public EditYCodeViewModel Map(WasteCodeDataAndNotificationData source)
        {
            return new EditYCodeViewModel
            {
                EnterWasteCodesViewModel = MapCodes(source, CodeType.Y)
            };
        }
    }
}