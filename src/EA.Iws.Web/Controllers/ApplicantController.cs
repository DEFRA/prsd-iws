namespace EA.Iws.Web.Controllers
{
    using Infrastructure.Authorization;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    [AuthorizeActivity(typeof(GetExportNotificationsByUser))]
    public class ApplicantController : Controller
    {
        private readonly IMediator mediator;

        public ApplicantController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Home()
        {
            var response = (await mediator.SendAsync(new GetExportNotificationsByUser())).ToList();

            return View(response);
        }
    }
}