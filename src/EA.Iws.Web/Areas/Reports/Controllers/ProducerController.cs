namespace EA.Iws.Web.Areas.Reports.Controllers
{
    using System.Web.Mvc;
    using ViewModels.Producer;

    public class ProducerController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View(new IndexViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(IndexViewModel model)
        {
            return View(model);
        }
    }
}