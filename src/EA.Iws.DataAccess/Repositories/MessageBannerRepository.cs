namespace EA.Iws.DataAccess.Repositories
{
    using EA.Iws.Domain;
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    internal class MessageBannerRepository : IMessageBannerRepository
    {
        private readonly IwsContext context;

        public MessageBannerRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<MessageBanner> GetMessageBannerData()
        {
            var currentDate = DateTime.UtcNow;

            return await context.MessageBanners
                                .Where(x => x.StartTime <= currentDate && x.EndTime >= currentDate)
                                .SingleOrDefaultAsync();
        }
    }
}
