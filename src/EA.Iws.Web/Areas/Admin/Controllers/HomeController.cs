namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using Api.Client;
    using Infrastructure;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels;

    [Authorize(Roles = "internal")]
    public class HomeController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public HomeController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new BasicSearchViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(BasicSearchViewModel model)
        {
            using (var client = apiClient())
            {
                var searchResults = await client.SendAsync(User.GetAccessToken(), model.ToRequest());
                if (searchResults != null)
                {
                    model.SearchResults = searchResults.ToList();
                }
                model.HasSearched = true;
            }
            return View(model);
        }
    }
}