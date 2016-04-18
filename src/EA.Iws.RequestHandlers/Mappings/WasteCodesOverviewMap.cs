namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Notification.Overview;
    using Core.WasteCodes;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;

    internal class WasteCodesOverviewMap : IMap<NotificationApplication, WasteCodesOverviewInfo>
    {
        private readonly IMapWithParameter<NotificationApplication, CodeType, WasteCodeData[]> wasteCodesMapper;

        public WasteCodesOverviewMap(IMapWithParameter<NotificationApplication, CodeType, WasteCodeData[]> wasteCodesMapper)
        {
            this.wasteCodesMapper = wasteCodesMapper;
        }

        public WasteCodesOverviewInfo Map(NotificationApplication notification)
        {
            return new WasteCodesOverviewInfo
            {
                NotificationId = notification.Id,
                BaselOecdCode = wasteCodesMapper.Map(notification, CodeType.Basel),
                EwcCodes = wasteCodesMapper.Map(notification, CodeType.Ewc),
                NationExportCode = wasteCodesMapper.Map(notification, CodeType.ExportCode),
                NationImportCode = wasteCodesMapper.Map(notification, CodeType.ImportCode),
                OtherCodes = wasteCodesMapper.Map(notification, CodeType.OtherCode),
                YCodes = wasteCodesMapper.Map(notification, CodeType.Y),
                HCodes = wasteCodesMapper.Map(notification, CodeType.H),
                UnClass = wasteCodesMapper.Map(notification, CodeType.Un),
                UnNumber = wasteCodesMapper.Map(notification, CodeType.UnNumber),
                CustomCodes = wasteCodesMapper.Map(notification, CodeType.CustomsCode)
            };
        }
    }
}
