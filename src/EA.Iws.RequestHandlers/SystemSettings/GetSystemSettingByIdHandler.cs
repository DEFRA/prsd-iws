namespace EA.Iws.RequestHandlers.SystemSettings
{
    using EA.Iws.Core.SystemSetting;
    using EA.Iws.Core.TechnologyEmployed;
    using EA.Iws.DataAccess.Repositories;
    using EA.Iws.Domain;
    using EA.Iws.Requests.SystemSettings;
    using EA.Prsd.Core.Mapper;
    using EA.Prsd.Core.Mediator;
    using System;
    using System.Threading.Tasks;

    internal class GetSystemSettingByIdHandler : IRequestHandler<GetSystemSettingById, SystemSettingData>
    {
        private readonly ISystemSettingRepository systemSettingRepository;
        private readonly IMap<SystemSetting, SystemSettingData> mapper;

        public GetSystemSettingByIdHandler(ISystemSettingRepository systemSettingRepository, IMap<SystemSetting, SystemSettingData> mapper)
        {
            this.systemSettingRepository = systemSettingRepository;
            this.mapper = mapper;
        }

        public async Task<SystemSettingData> HandleAsync(GetSystemSettingById message)
        {
            var systemSetting = await systemSettingRepository.GetById(message.SystemSettingId);

            return mapper.Map(systemSetting);
        }
    }
}