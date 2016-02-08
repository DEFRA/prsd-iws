namespace EA.Iws.Web.Infrastructure.Authorization
{
    using System;
    using System.Threading.Tasks;
    using Core.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Authorization;

    public class AuthorizationService
    {
        private readonly RequestAuthorizationAttributeCache attributeCache;
        private readonly IMediator mediator;

        public AuthorizationService(IMediator mediator, 
            RequestAuthorizationAttributeCache attributeCache)
        {
            this.mediator = mediator;
            this.attributeCache = attributeCache;
        }

        public async Task<bool> AuthorizeActivity(string activity)
        {
            return await mediator.SendAsync(new AuthorizeActivity(activity));
        }

        public async Task<bool> AuthorizeActivity(Type requestType)
        {
            var attribute = attributeCache.Get(requestType);

            if (attribute != null)
            {
                var activity = attribute.Name;

                return await AuthorizeActivity(activity);
            }
            else
            {
                return true;
            }
        }
    }
}