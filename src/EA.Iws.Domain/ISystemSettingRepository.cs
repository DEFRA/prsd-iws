namespace EA.Iws.Domain
{
    using System.Threading.Tasks;

    public interface ISystemSettingRepository
    {
        Task<SystemSetting> GetById(int id);
    }
}