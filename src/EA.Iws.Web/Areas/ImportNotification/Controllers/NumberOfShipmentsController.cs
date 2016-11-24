namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System.Web.Mvc;
    using Core.Authorization.Permissions;
    using Infrastructure.Authorization;

    [AuthorizeActivity(ImportNotificationPermissions.CanChangeNumberOfShipmentsOnImportNotification)]
    public class NumberOfShipmentsController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}