namespace EA.Iws.Web.Areas.ExportMovement.Controllers
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

            var proposedDate = await mediator.SendAsync(new IsProposedUpdatedMovementDateValid(id, model.AsDateTime().Value));

            if (proposedDate.IsOutsideConsentPeriod)
            {
                ModelState.AddModelError("Day",
                    "The actual date of shipment cannot be outside of the consent validity period. Please enter a different date.");
            }

            if (proposedDate.IsOutOfRange)
            {
                ModelState.AddModelError("Day",
                    "The actual date of shipment cannot be more than 30 calendar days in the future. Please enter a different date.");
            }

            if (proposedDate.IsOutOfRangeOfOriginalDate)
            {
                ModelState.AddModelError("Day",
                    "The actual date of shipment cannot be more than 10 working days after the original date. Please enter a different date.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var notificationId = await mediator.SendAsync(new GetNotificationIdByMovementId(id));

            await mediator.SendAsync(new UpdateMovementDate(id, model.AsDateTime().Value));

            return RedirectToAction("Index", "Options", new { area = "NotificationApplication", id = notificationId });
        }
    }
}