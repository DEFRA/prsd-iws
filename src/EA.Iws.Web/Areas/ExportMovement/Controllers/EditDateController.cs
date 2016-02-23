namespace EA.Iws.Web.Areas.ExportMovement.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Requests.Movement.Edit;
    using Requests.NotificationMovements;
    using ViewModels.EditDate;

    [AuthorizeActivity(typeof(UpdateMovementDate))]
    public class EditDateController : Controller
    {
        private readonly IMediator mediator;
        private const string ShipmentDateKey = "ShipmentDateKey";

        public EditDateController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var dateHistories = await mediator.SendAsync(new GetMovementDateHistory(id));
            var notificationId = await mediator.SendAsync(new GetNotificationIdByMovementId(id));

            var model = new EditDateViewModel
            {
                DateEditHistory = dateHistories
                    .OrderBy(dh => dh.DateChanged)
                    .Select(dh => dh.PreviousDate)
                    .ToList(),
                NotificationId = notificationId
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

            var workingDaysUntilShipment = await mediator.SendAsync(new GetWorkingDaysUntil(notificationId, model.AsDateTime().GetValueOrDefault()));

            if (workingDaysUntilShipment < 4)
            {
                TempData[ShipmentDateKey] = model.AsDateTime();
                return RedirectToAction("ThreeWorkingDaysWarning", "EditDate");
            }

            await mediator.SendAsync(new UpdateMovementDate(id, model.AsDateTime().Value));

            return RedirectToAction("Index", "Options", new { area = "NotificationApplication", id = notificationId });
        }

        [HttpGet]
        public ActionResult ThreeWorkingDaysWarning(Guid id)
        {
            object result;
            if (TempData.TryGetValue(ShipmentDateKey, out result))
            {
                var model = new ThreeWorkingDaysWarningViewModel((DateTime)result);

                return View("ThreeWorkingDays", model);
            }

            return RedirectToAction("Index", "EditDate");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ThreeWorkingDaysWarning(Guid id, ThreeWorkingDaysWarningViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ThreeWorkingDays", model);
            }

            if (model.Selection == ThreeWorkingDaysSelection.ChangeDate)
            {
                return RedirectToAction("Index", "EditDate");
            }

            await mediator.SendAsync(new UpdateMovementDate(id, model.ShipmentDate));

            var notificationId = await mediator.SendAsync(new GetNotificationIdByMovementId(id));

            return RedirectToAction("Index", "Options", new { area = "NotificationApplication", id = notificationId });
        }
    }
}