namespace EA.Iws.Requests.SystemSettings
{
    using EA.Iws.Core.SystemSetting;
    using EA.Prsd.Core.Mediator;

    public class GetSystemSettingById : IRequest<SystemSettingData>
    {
        public GetSystemSettingById(int systemSettingId)
        {
            SystemSettingId = systemSettingId;
        }

        public int SystemSettingId { get; private set; }
    }
}