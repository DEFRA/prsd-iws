namespace EA.Iws.Web.Infrastructure.Authorization
{
    using System;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using Prsd.Core;

    public class AuthorizeActivityAttribute : AuthorizeAttribute
    {
        private readonly Type requestType;

        private readonly string activity;

        public AuthorizationService AuthorizationService { get; set; }

        public AuthorizeActivityAttribute(Type requestType)
        {
            Guard.ArgumentNotNull(() => requestType, requestType);

            this.requestType = requestType;
        }

        public AuthorizeActivityAttribute(string activity)
        {
            Guard.ArgumentNotNullOrEmpty(() => activity, activity);

            this.activity = activity;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!string.IsNullOrWhiteSpace(activity))
            {
                return Task.Run(() => AuthorizationService.AuthorizeActivity(activity)).Result;
            }

            return Task.Run(() => AuthorizationService.AuthorizeActivity(requestType)).Result;
        }
    }
}