namespace EA.Iws.Web.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.Registration;

    public class RegistrationController : Controller
    {
        public ActionResult ApplicantRegistration()
        {
            return View(new ApplicantRegistrationViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitApplicantRegistration(ApplicantRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ApplicantRegistration", model);
            }

            return View("ApplicantRegistration", model);
        }
    }
}