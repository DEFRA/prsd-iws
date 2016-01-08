namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;
    using ViewModels.Menu;

    [Authorize(Roles = "internal")]
    public class MenuController : Controller
    {
        private readonly IMediator mediator;

        public MenuController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult ImportNavigation(Guid id, ImportNavigationSection section)
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult ExportNavigation(Guid id, ExportNavigationSection section)
        {
            var data = Task.Run(() => mediator.SendAsync(new GetNotificationAssessmentSummaryInformation(id))).Result;

            var model = new ExportNavigationViewModel
            {
                Data = data,
                ActiveSection = section
            };

            return PartialView("_ExportNavigation", model);
        }
    }
}