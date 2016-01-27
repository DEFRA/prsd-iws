namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.Admin;
    using ViewModels.ExportNotifications;

    [Authorize(Roles = "internal")]
    public class ExportNotificationsController : Controller
    {
        private readonly IMediator mediator;

        public ExportNotificationsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var tableData = await mediator.SendAsync(new GetExportNotificationOwnerDisplays());

            var model = new ExportNotificationsViewModel { TableData = tableData };

            return View(model);
        }
    }
}