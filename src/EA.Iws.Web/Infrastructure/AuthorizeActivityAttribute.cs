namespace EA.Iws.Web.Infrastructure
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using Core.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Authorization;

    public class AuthorizeActivityAttribute : AuthorizeAttribute
    {
        private readonly Type requestType;

        public IMediator Mediator { get; set; }

        public AuthorizeActivityAttribute(Type requestType)
        {
            this.requestType = requestType;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var attr = requestType.GetCustomAttribute<RequestAuthorizationAttribute>();

            if (attr != null)
            {
                var activity = attr.Name;

                return Task.Run(() => Mediator.SendAsync(new AuthorizeActivity(activity))).Result;
            }
            else
            {
                return true;
            }
        }
    }
}