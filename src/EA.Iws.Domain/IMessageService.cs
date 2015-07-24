namespace EA.Iws.Domain
{
    using System.Threading.Tasks;

    public interface IMessageService
    {
        Task SendAsync<T>(T message) where T : class;
    }
}
