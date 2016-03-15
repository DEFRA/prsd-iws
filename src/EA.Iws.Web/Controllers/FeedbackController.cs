namespace EA.Iws.Web.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Services;
    using ViewModels.NewUser;

    public class FeedbackController : Controller
    {
        private readonly IMediator mediator;
        private readonly AppConfiguration config;

        public FeedbackController(IMediator mediator, AppConfiguration config)
        {
            this.mediator = mediator;
            this.config = config;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (!string.IsNullOrWhiteSpace(config.DonePageUrl))
            {
                return Redirect(config.DonePageUrl);
            }

            var model = new FeedbackViewModel();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(FeedbackViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await mediator.SendAsync(model.ToRequest());
            return View("FeedbackSent");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult FeedbackSent()
        {
            return View();
        }
    }
}