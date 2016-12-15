namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System;
    using System.Web.Mvc;
    using Infrastructure.Authorization;
    using Requests.NotificationAssessment;

    [AuthorizeActivity(typeof(DeleteExportNotification))]
    public class DeleteNotificationController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}