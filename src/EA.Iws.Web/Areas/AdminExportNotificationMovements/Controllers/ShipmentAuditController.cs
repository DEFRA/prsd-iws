namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Requests.Notification;
    using ViewModels.ShipmentAudit;
    using Web.ViewModels;

    [AuthorizeActivity(typeof(GetMovementAuditByNotificationId))]
    public class ShipmentAuditController : Controller
    {
        private readonly IMediator mediator;

        public ShipmentAuditController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, ShipmentAuditFilterType? filter, int? number = null, int page = 1)
        {
            number = filter.HasValue && filter == ShipmentAuditFilterType.ShipmentNumber ? number : null;

            var response = await mediator.SendAsync(new GetMovementAuditByNotificationId(id, page, number));

            var model = new ShipmentAuditViewModel(response)
            {
                NotificationId = id,
                SelectedFilter = filter,
                ShipmentNumberSearch = number.HasValue ? number.ToString() : null,
                NotificationNumber = await mediator.SendAsync(new GetNotificationNumber(id))
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, ShipmentAuditViewModel model)
        {
            if (model.SelectedFilter == null)
            {
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                var selectedFilter = model.SelectedFilter;
                var response = await mediator.SendAsync(new GetMovementAuditByNotificationId(id, 1));
                model = new ShipmentAuditViewModel(response)
                {
                    NotificationId = id,
                    SelectedFilter = selectedFilter,
                    NotificationNumber = await mediator.SendAsync(new GetNotificationNumber(id))
                };
                return View(model);
            }

            return RedirectToAction("Index", new { id = id, filter = model.SelectedFilter, number = model.ShipmentNumber, page = 1 });
        }
    }
}