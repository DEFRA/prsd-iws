namespace EA.Iws.Web.Controllers
{
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

            return RedirectToAction("WasteOperation", 
                new { ca = model.CompetentAuthorities.SelectedValue });
        }
    }
}