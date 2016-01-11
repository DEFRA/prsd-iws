namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.ImportNotificationAssessment;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
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
            var details = Task.Run(() => mediator.SendAsync(new GetNotificationDetails(id))).Result;

            var model = new ImportNavigationViewModel
            {
                Details = details,
                ActiveSection = section,
                ShowImportSections = details.Status == ImportNotificationStatus.NotificationReceived
            };

            return PartialView("_ImportNavigation", model);
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