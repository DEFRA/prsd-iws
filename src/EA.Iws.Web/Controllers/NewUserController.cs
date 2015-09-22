namespace EA.Iws.Web.Controllers
{
    using System.Web.Mvc;

    public class NewUserController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Cookies()
        {
            return View();
        }
    }
}