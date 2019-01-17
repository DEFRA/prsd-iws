namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;

    public class PrenotificationBulkUploadController : Controller
    {
        private readonly IMediator mediator;

        public PrenotificationBulkUploadController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult UploadPrenotifications()
        {
            var model = new NoChangesViewModel();

            return View(null);
        }
    }
}