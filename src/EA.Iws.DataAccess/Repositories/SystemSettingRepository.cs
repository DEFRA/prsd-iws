namespace EA.Iws.DataAccess.Repositories
{
    using EA.Iws.Core.Notification;
    using EA.Iws.Core.SystemSettings;
    using EA.Iws.Domain;
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    public class SystemSettingRepository : ISystemSettingRepository
    {
        private readonly IwsContext context;

        public SystemSettingRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<SystemSetting> GetById(SystemSettingType id)
        {
            try
            {
                return await context.SystemSettings.SingleAsync(ss => ss.Id == (int)id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<SystemSetting> GetSystemSettings(UKCompetentAuthority competentAuthority, SystemSettingType systemSettingType)
        {
            try
            {
                return await context.SystemSettings.Where(ss => ss.CompetentAuthority == competentAuthority && ss.PriceType == systemSettingType)
                                                   .OrderByDescending(ss => ss.ValidFrom)
                                                   .Take(1)
                                                   .SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }
    }
}