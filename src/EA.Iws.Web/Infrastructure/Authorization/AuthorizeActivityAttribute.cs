namespace EA.Iws.Web.Infrastructure.Authorization
{
    using System;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;

    public class AuthorizeActivityAttribute : AuthorizeAttribute
    {
        private readonly Type requestType;

        private readonly string activity;

        public AuthorizationService AuthorizationService { get; set; }

        public AuthorizeActivityAttribute(Type requestType)
        {
            this.requestType = requestType;
        }

        public AuthorizeActivityAttribute(string activity)
        {
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