namespace EA.Iws.Api.Identity
{
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;

    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Connect your email provider.
            return Task.FromResult(0);
        }
    }
}