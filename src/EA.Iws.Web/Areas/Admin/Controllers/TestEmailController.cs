namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.Admin;
    using ViewModels.TestEmail;

    [Authorize(Roles = "internal")]
    public class TestEmailController : Controller
    {
        private readonly IMediator mediator;

        public TestEmailController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(TestEmailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await mediator.SendAsync(new SendTestEmail(model.EmailTo));

            if (result)
            {
                return RedirectToAction("Success");
            }

            ModelState.AddModelError(string.Empty, TestEmailResources.ErrorSendingEmail);
            return View(model);
        }

        [HttpGet]
        public ActionResult Success()
        {
            return View();
        }
    }
}