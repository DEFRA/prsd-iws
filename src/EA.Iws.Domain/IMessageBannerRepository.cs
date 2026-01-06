namespace EA.Iws.Domain
{
    using System.Threading.Tasks;

    public interface IMessageBannerRepository
    {
        Task<MessageBanner> GetMessageBannerData();
    }
}
