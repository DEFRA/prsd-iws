namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using EA.Iws.Requests.Notification;
    using EA.Iws.Web.Areas.Admin.ViewModels.ArchiveNotification;
    using EA.Iws.Web.Infrastructure.Authorization;
    using EA.Prsd.Core.Mediator;
    using Requests.Admin.ArchiveNotification;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    [AuthorizeActivity(typeof(ArchiveNotifications))]
    public class ArchiveNotificationController : Controller
    {
        private readonly IMediator mediator;

        public ArchiveNotificationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        // GET: Admin/ArchiveNotification
        [HttpGet]
        public async Task<ActionResult> Index(int page = 1)
        {
            var response = await mediator.SendAsync(new GetArchiveNotificationsByUser(page));

            var model = new ArchiveNotificationResultViewModel(response);

            return View(model);
        }
    }
}