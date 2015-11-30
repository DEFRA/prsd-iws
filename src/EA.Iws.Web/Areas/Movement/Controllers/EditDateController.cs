namespace EA.Iws.Web.Areas.Movement.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Requests.Movement.Edit;
    using ViewModels.EditDate;

    [Authorize]
    public class EditDateController : Controller
    {
        private readonly IMediator mediator;

        public EditDateController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var dateHistories = await mediator.SendAsync(new GetMovementDateHistory(id));

            var model = new EditDateViewModel
            {
                DateEditHistory = dateHistories
                    .OrderBy(dh => dh.DateChanged)
                    .Select(dh => dh.PreviousDate)
                    .ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, EditDateViewModel model)
        {
            if (!ModelState.IsValid || !model.AsDateTime().HasValue)
            {
                return View(model);
            }

            var validDates = await mediator.SendAsync(new GetMaximumValidMovementDate(id));

            if (model.AsDateTime().Value > validDates.ConsentEnd)
            {
                ModelState.AddModelError("Day", string.Format(
                    "Please enter a date that is within the consent range of {0:dd MMM yyyy} to {1:dd MMM yyyy}",
                    validDates.ConsentStart,
                    validDates.ConsentEnd));
                return View(model);
            }

            if (model.AsDateTime().Value > validDates.MaxValidDate)
            {
                ModelState.AddModelError("Day", "Please enter a date that is within 10 working days of the original actual shipment date");
                return View(model);
            }

            var notificationId = await mediator.SendAsync(new GetNotificationIdByMovementId(id));

            await mediator.SendAsync(new UpdateMovementDate(id, model.AsDateTime().Value));

            return RedirectToAction("Index", "Home", new { area = "NotificationMovements", notificationId });
        }
    }
}