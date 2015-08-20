namespace EA.Iws.Web.Mappings.WasteCodes
{
    using System.Collections.Generic;
    using System.Linq;
    using Areas.NotificationApplication.ViewModels.WasteCodes;
    using Areas.NotificationApplication.Views.Shared;
    using Core.WasteCodes;
    using Prsd.Core.Mapper;
    using Requests.WasteCodes;

    public abstract class WasteCodeMapBase
    {
        private readonly IMap<IEnumerable<WasteCodeData>, IList<WasteCodeViewModel>> mapper;

        protected WasteCodeMapBase(IMap<IEnumerable<WasteCodeData>, IList<WasteCodeViewModel>> mapper)
        {
            this.mapper = mapper;
        }

        protected virtual EnterWasteCodesViewModel MapCodes(WasteCodeDataAndNotificationData source, CodeType codeType)
        {
            return new EnterWasteCodesViewModel
            {
                IsNotApplicable = source.NotApplicableCodes.Any(nac => nac == codeType),
                SelectedWasteCodes =
                    source.NotificationWasteCodeData[codeType].Select(wc => wc.Id).ToList(),
                WasteCodes = mapper.Map(source.LookupWasteCodeData[codeType])
            };
        }
    }
}