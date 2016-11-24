namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System.Web.Mvc;
    using Core.Authorization.Permissions;
    using Infrastructure.Authorization;

    [AuthorizeActivity(ExportNotificationPermissions.CanChangeNumberOfShipmentsOnExportNotification)]
    public class NumberOfShipmentsController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}