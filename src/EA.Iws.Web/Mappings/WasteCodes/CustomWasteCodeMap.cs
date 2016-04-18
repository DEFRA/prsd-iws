namespace EA.Iws.Web.Mappings.WasteCodes
{
    using System.Linq;
    using Areas.NotificationApplication.ViewModels.CustomWasteCode;
    using Core.WasteCodes;
    using Prsd.Core.Mapper;
    using Requests.WasteCodes;

    public class CustomWasteCodeMap : IMap<WasteCodeDataAndNotificationData, CustomWasteCodesViewModel>
    {
        public CustomWasteCodesViewModel Map(WasteCodeDataAndNotificationData source)
        {
            var exportCode = source.NotificationWasteCodeData[CodeType.ExportCode].SingleOrDefault();
            var importCode = source.NotificationWasteCodeData[CodeType.ImportCode].SingleOrDefault();
            var customsCode = source.NotificationWasteCodeData[CodeType.CustomsCode].SingleOrDefault();
            var otherCode = source.NotificationWasteCodeData[CodeType.OtherCode].SingleOrDefault();

            return new CustomWasteCodesViewModel
            {
                ExportNationalCode = GetWasteCode(exportCode),
                ExportNationalCodeNotApplicable = IsCodeNotApplicable(CodeType.ExportCode, source),
                ImportNationalCode = GetWasteCode(importCode),
                ImportNationalCodeNotApplicable = IsCodeNotApplicable(CodeType.ImportCode, source),
                CustomsCode = GetWasteCode(customsCode),
                CustomsCodeNotApplicable = IsCodeNotApplicable(CodeType.CustomsCode, source),
                OtherCode = GetWasteCode(otherCode),
                OtherCodeNotApplicable = IsCodeNotApplicable(CodeType.OtherCode, source)
            };
        }

        private string GetWasteCode(WasteCodeData wasteCode)
        {
            return (wasteCode == null) ? null : wasteCode.Code;
        }

        private bool IsCodeNotApplicable(CodeType codeType, WasteCodeDataAndNotificationData source)
        {
            return source.NotApplicableCodes.Any(nac => nac == codeType);
        }
    }
}