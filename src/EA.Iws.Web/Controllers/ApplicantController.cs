namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;

    public class ApplicantController : Controller
    {
        // GET: Applicant
        public async Task<ActionResult> Home()
        {
            return View();
        }
    }
}