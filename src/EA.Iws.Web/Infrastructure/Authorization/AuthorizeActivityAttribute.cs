namespace EA.Iws.Web.Infrastructure.Authorization
{
    using System;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;

    public class AuthorizeActivityAttribute : AuthorizeAttribute
    {
        private readonly Type requestType;

        public AuthorizationService AuthorizationService { get; set; }

        public AuthorizeActivityAttribute(Type requestType)
        {
            this.requestType = requestType;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return Task.Run(() => AuthorizationService.AuthorizeActivity(requestType)).Result;
        }
    }
}