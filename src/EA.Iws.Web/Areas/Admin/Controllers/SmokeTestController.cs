namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.Admin;

    [AllowAnonymous]
    public class SmokeTestController : Controller
    {
        private readonly IMediator mediator;

        public SmokeTestController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var info = await mediator.SendAsync(new SmokeTest());
            return Json(info, JsonRequestBehavior.AllowGet);
        }
    }
}