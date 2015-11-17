namespace EA.Iws.Web.Areas.Movement.Controllers
{
    using System;
    using System.Web.Mvc;

    [Authorize]
    public class EditDateController : Controller
    {
        [HttpGet]
        public ActionResult Index(Guid id)
        {
            return View();
        }
    }
}