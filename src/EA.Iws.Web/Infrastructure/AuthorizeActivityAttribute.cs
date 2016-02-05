namespace EA.Iws.Web.Infrastructure
{
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.Authorization;

    public class AuthorizeActivityAttribute : AuthorizeAttribute
    {
        private readonly string activity;

        public IMediator Mediator { get; set; }

        public AuthorizeActivityAttribute(string activity)
        {
            this.activity = activity;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return Task.Run(() => Mediator.SendAsync(new AuthorizeActivity(activity))).Result;
        }
    }
}