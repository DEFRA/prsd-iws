namespace EA.Iws.Web.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Antlr.Runtime.Misc;
    using Api.Client;
    using ViewModels.NewUser;

    public class FeedbackController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public FeedbackController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
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

            using (var client = apiClient())
            {
                await client.SendAsync(model.ToRequest());
                return View("FeedbackSent");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult FeedbackSent()
        {
            return View();
        }
    }
}