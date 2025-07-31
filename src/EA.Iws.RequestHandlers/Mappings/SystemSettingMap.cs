namespace EA.Iws.RequestHandlers.Mappings
{
    using EA.Iws.Core.SystemSetting;
    using EA.Iws.Domain;
    using EA.Prsd.Core.Mapper;

    internal class SystemSettingMap : IMap<SystemSetting, SystemSettingData>
    {
        public SystemSettingMap()
        {
        }

        public SystemSettingData Map(SystemSetting source)
        {
            return new SystemSettingData
            {
                Id = source.Id,
                Value = source.Price
            };
        }
    }
}