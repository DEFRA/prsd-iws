namespace EA.Iws.Web.Infrastructure
{
    using System.Threading.Tasks;
    using Prsd.Core.Mediator;
    using Requests.Security;

    public class AuthorizationService
    {
        private readonly IMediator mediator;

        public AuthorizationService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<bool> AuthorizeActivity(string activity)
        {
            return await mediator.SendAsync(new AuthorizeActivity(activity));
        }
    }
}