namespace EA.Iws.Web.Mappings.NotificationAssessment
{
    using System.Collections.Generic;
    using Areas.AdminExportAssessment.ViewModels.HCode;
    using Areas.NotificationApplication.Views.Shared;
    using Core.WasteCodes;
    using Prsd.Core.Mapper;
    using Requests.WasteCodes;
    using WasteCodes;

    public class EditHCodeMap : WasteCodeMapBase, IMap<WasteCodeDataAndNotificationData, EditHCodeViewModel>
    {
        public EditHCodeMap(IMap<IEnumerable<WasteCodeData>, IList<WasteCodeViewModel>> mapper) : base(mapper)
        {
        }

        public EditHCodeViewModel Map(WasteCodeDataAndNotificationData source)
        {
            return new EditHCodeViewModel
            {
                EnterWasteCodesViewModel = MapCodes(source, CodeType.H)
            };
        }
    }
}