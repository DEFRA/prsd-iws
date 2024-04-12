namespace EA.Iws.Domain
{
    using EA.Iws.Core.SystemSettings;
    using System.Threading.Tasks;

    public interface ISystemSettingRepository
    {
        Task<SystemSetting> GetById(SystemSettingType id);
    }
}