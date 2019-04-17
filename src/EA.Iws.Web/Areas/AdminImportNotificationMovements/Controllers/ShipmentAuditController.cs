namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement;
    using Requests.ImportNotification;
    using ViewModels.ShipmentAudit;

    [AuthorizeActivity(typeof(GetImportMovementAuditByNotificationId))]
    public class ShipmentAuditController : Controller
    {
        private readonly IMediator mediator;

        public ShipmentAuditController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, int page = 1)
        {
            var response = await mediator.SendAsync(new GetImportMovementAuditByNotificationId(id, page));
            var model = new ShipmentAuditViewModel(response);
            model.NotificationId = id;
            model.NotificationNumber = await mediator.SendAsync(new GetImportNotificationNumberById(id));
            return View(model);
        }
    }
}