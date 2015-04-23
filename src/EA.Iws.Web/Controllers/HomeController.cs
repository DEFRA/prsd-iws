namespace EA.Iws.Web.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        // GET: Home
        public async Task<ActionResult> Index()
        {
            return View();
        }
    }
}