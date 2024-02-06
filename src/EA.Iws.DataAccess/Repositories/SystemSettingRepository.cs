namespace EA.Iws.DataAccess.Repositories
{
    using EA.Iws.Domain;
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;

    public class SystemSettingRepository : ISystemSettingRepository
    {
        private readonly IwsContext context;

        public SystemSettingRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<SystemSetting> GetById(int id)
        {
            try
            {
                return await context.SystemSettings.SingleAsync(ss => ss.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }
    }
}