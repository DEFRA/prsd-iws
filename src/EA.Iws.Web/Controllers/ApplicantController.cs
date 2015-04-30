namespace EA.Iws.Web.Controllers
{
    using System.Web.Mvc;

    [Authorize]
    public class ApplicantController : Controller
    {
        // GET: Applicant
        public ActionResult Home()
        {
            return View();
        }
    }
}