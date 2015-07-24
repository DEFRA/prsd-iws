namespace EA.Iws.TestHelpers
{
    using System.Threading.Tasks;
    using Domain;

    public class TestMessageService : IMessageService
    {
        public Task SendAsync<T>(T message) where T : class
        {
            return Task.FromResult(0);
        }
    }
}