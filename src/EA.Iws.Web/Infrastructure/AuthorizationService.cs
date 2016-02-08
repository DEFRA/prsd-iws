namespace EA.Iws.Web.Infrastructure
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using Core.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Authorization;

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

        public async Task<bool> AuthorizeActivity(Type requestType)
        {
            var attr = requestType.GetCustomAttribute<RequestAuthorizationAttribute>();

            if (attr != null)
            {
                var activity = attr.Name;

                return await mediator.SendAsync(new AuthorizeActivity(activity));
            }
            else
            {
                return true;
            }
        }
    }
}