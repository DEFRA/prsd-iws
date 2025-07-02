namespace EA.Iws.Requests.SystemSettings
{
    using EA.Iws.Core.Notification;
    using EA.Iws.Core.SystemSetting;
    using EA.Iws.Core.SystemSettings;
    using EA.Prsd.Core.Mediator;

    public class GetSystemSettings : IRequest<SystemSettingData>
    {
        public GetSystemSettings(UKCompetentAuthority competentAuthority, SystemSettingType priceTypeId)
        {
            CompetentAuthority = competentAuthority;
            PriceTypeId = priceTypeId;
        }

        public SystemSettingType PriceTypeId { get; private set; }

        public UKCompetentAuthority CompetentAuthority { get; private set; }
    }
}
