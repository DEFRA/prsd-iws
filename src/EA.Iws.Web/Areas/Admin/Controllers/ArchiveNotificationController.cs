namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using EA.Iws.Web.Areas.Admin.ViewModels.ArchiveNotification;
    using EA.Iws.Web.Infrastructure.Authorization;
    using Requests.Admin.ArchiveNotification;
    using System.Web.Mvc;    

    [AuthorizeActivity(typeof(ArchiveNotifications))]
    public class ArchiveNotificationController : Controller
    {
        // GET: Admin/ArchiveNotification
        public ActionResult Index()
        {
            var model = new ArchiveNotificationResultViewModel();

            return View(model);
        }
    }
}