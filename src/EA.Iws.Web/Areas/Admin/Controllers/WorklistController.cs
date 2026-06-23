namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;
    using ViewModels.Worklist;

    [AuthorizeActivity(typeof(GetExportWorklist))]
    public class WorklistController : Controller
    {
        private readonly IMediator mediator;

        public WorklistController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(ExportWorklistFilterViewModel filter, int page = 1)
        {
            var model = new WorklistViewModel
            {
                ExportsFilter = filter ?? new ExportWorklistFilterViewModel(),
                ExportsResult = await mediator.SendAsync(new GetExportWorklist
                {
                    NotificationNumber = filter == null ? null : filter.NotificationNumber,
                    Officer = filter == null ? null : filter.Officer,
                    Status = filter == null ? null : filter.Status,
                    PageNumber = page
                })
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(WorklistViewModel model)
        {
            return RedirectToAction("Index", new
            {
                notificationNumber = model.ExportsFilter.NotificationNumber,
                officer = model.ExportsFilter.Officer,
                status = model.ExportsFilter.Status,
                page = 1
            });
        }
    }
}