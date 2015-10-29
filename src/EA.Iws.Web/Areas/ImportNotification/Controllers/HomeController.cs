namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System;
    using System.Web.Mvc;

    [Authorize(Roles = "internal")]
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index(Guid id)
        {
            return HttpNotFound();
        }
    }
}