namespace EA.Iws.Requests.SystemSettings
{
    using EA.Iws.Core.Authorization;
    using EA.Iws.Core.Authorization.Permissions;
    using EA.Iws.Core.SystemSetting;
    using EA.Iws.Core.SystemSettings;
    using EA.Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanViewSearchResults)]
    public class GetSystemSettingById : IRequest<SystemSettingData>
    {
        public GetSystemSettingById(SystemSettingType systemSettingId)
        {
            SystemSettingId = systemSettingId;
        }

        public SystemSettingType SystemSettingId { get; private set; }
    }
}