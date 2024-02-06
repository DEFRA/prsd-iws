namespace EA.Iws.Requests.SystemSettings
{
    using EA.Iws.Core.Authorization;
    using EA.Iws.Core.Authorization.Permissions;
    using EA.Iws.Core.SystemSetting;
    using EA.Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanViewSearchResults)]
    public class GetSystemSettingById : IRequest<SystemSettingData>
    {
        public GetSystemSettingById(int systemSettingId)
        {
            SystemSettingId = systemSettingId;
        }

        public int SystemSettingId { get; private set; }
    }
}