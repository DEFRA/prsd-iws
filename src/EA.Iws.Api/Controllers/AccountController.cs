namespace EA.Iws.Api.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Http;
    using Client.Entities;
    using Identity;
    using Microsoft.AspNet.Identity;

    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private readonly ApplicationUserManager userManager;

        public AccountController(ApplicationUserManager userManager)
        {
            this.userManager = userManager;
        }
    }
}