namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using ViewModels.NotificationApplication;
    using ViewModels.Shared;

    public class NotificationApplicationController : Controller
    {
        [HttpGet]
        public ActionResult CompetentAuthority()
        {
            var model = new CompetentAuthorityChoice
            {
                CompetentAuthorities =
                    RadioButtonStringCollection.CreateFromEnum<UKCompetentAuthorities>()
            };

            return View("CompetentAuthority", model);
        }

        public ActionResult CompetentAuthority(CompetentAuthorityChoice model)
        {
            if (!ModelState.IsValid)
            {
                return View("CompetentAuthority", model);
            }

            return RedirectToAction("WasteActionQuestion",
                new { ca = model.CompetentAuthorities.SelectedValue });
        }

        public ActionResult WasteActionQuestion(string ca, string nt)
        {
            InitialQuestions model = new InitialQuestions
            {
                SelectedWasteAction = WasteAction.Recovery.ToString(),
                CompetentAuthority = ca,
                CompetentAuthorityContactInfo = GetCompetentAuthorityContactInfo(ca)
            };

            if (!string.IsNullOrWhiteSpace(nt))
            {
                model.SelectedWasteAction =
                    (nt.Equals(WasteAction.Disposal.ToString(), StringComparison.InvariantCultureIgnoreCase))
                        ? WasteAction.Disposal.ToString()
                        : WasteAction.Recovery.ToString();
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult WasteActionQuestion(InitialQuestions model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.SelectedWasteAction))
            {
                ModelState.AddModelError(string.Empty, "Please select notification type");
                return View(model);
            }

            return RedirectToAction("GenerateNumber",
                new
                {
                    ca = model.CompetentAuthority,
                    nt = model.SelectedWasteAction
                });
        }

        private string GetCompetentAuthorityContactInfo(string ca)
        {
            if (ca == null)
            {
                return string.Empty;
            }

            if (ca.Equals("Environment Agency"))
            {
                return "Environment Agency - Tel: 01253 876934";
            }

            if (ca.Equals("Scottish Environment Protection Agency"))
            {
                return "Scottish Environment Protection Agency - Tel: 01253 876934";
            }

            if (ca.Equals("Northern Ireland Environment Agency"))
            {
                return "Northern Ireland Environment Agency - Tel: 01253 876934";
            }

            if (ca.Equals("Natural Resources Wales"))
            {
                return "Natural Resources Wales - Tel: 01253 876934";
            }

            return string.Empty;
        }

        public ActionResult GenerateNumber(int? ca, string nt)
        {
            ViewBag.nt = nt;
            return View();
        }
    }
}