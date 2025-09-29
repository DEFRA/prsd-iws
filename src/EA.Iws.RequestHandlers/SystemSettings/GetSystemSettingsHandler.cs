namespace EA.Iws.RequestHandlers.SystemSettings
{
    using EA.Iws.Core.SystemSetting;
    using EA.Iws.Domain;
    using EA.Iws.Requests.SystemSettings;
    using EA.Prsd.Core.Mapper;
    using EA.Prsd.Core.Mediator;
    using System.Threading.Tasks;

    internal class GetSystemSettingsHandler : IRequestHandler<GetSystemSettings, SystemSettingData>
    {
        private readonly ISystemSettingRepository systemSettingRepository;
        private readonly IMap<SystemSetting, SystemSettingData> mapper;

        public GetSystemSettingsHandler(ISystemSettingRepository systemSettingRepository, IMap<SystemSetting, SystemSettingData> mapper)
        {
            this.systemSettingRepository = systemSettingRepository;
            this.mapper = mapper;
        }

        public async Task<SystemSettingData> HandleAsync(GetSystemSettings systemSettings)
        {
            var systemSetting = await systemSettingRepository.GetSystemSettings(systemSettings.CompetentAuthority, systemSettings.PriceTypeId);

            return mapper.Map(systemSetting);
        }
    }
}
