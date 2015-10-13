namespace EA.Iws.Web.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using ViewModels.NewUser;

    public class FeedbackController : Controller
    {
        private readonly IMediator mediator;

        public FeedbackController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
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