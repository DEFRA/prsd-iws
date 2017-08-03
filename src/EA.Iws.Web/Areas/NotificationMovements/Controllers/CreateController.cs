namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Movement;
    using Core.Rules;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Requests.Notification;
    using Requests.NotificationMovements;
    using Requests.NotificationMovements.Create;
    using ViewModels.Create;

    [AuthorizeActivity(typeof(CreateMovementAndDetails))]
    public class CreateController : Controller
    {
        private readonly IMediator mediator;

        public CreateController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid notificationId)
        {
            var ruleSummary = await mediator.SendAsync(new GetMovementRulesSummary(notificationId));

            if (!ruleSummary.IsSuccess)
            {
                return GetRuleErrorView(ruleSummary);
            }

            var shipmentDates = await mediator.SendAsync(new GetShipmentDates(notificationId));
            var shipmentUnits = await mediator.SendAsync(new GetShipmentUnits(notificationId));
            var availablePackagingTypes = await mediator.SendAsync(new GetPackagingTypes(notificationId));

            var model = new CreateMovementsViewModel(shipmentDates, shipmentUnits, availablePackagingTypes);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid notificationId, CreateMovementsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var proposedMovementDate = await mediator.SendAsync(new IsProposedMovementDateValid(notificationId, model.AsDateTime().Value));

            if (proposedMovementDate.IsOutOfRange)
            {
                ModelState.AddModelError("Day",
                    "The actual date of shipment cannot be more than 30 calendar days in the future. Please enter a different date.");
            }

            if (proposedMovementDate.IsOutsideConsentPeriod)
            {
                ModelState.AddModelError("Day",
                    "The actual date of shipment cannot be outside of the consent validity period. Please enter a different date.");
            }

            var workingDaysUntilShipment = await mediator.SendAsync(new GetWorkingDaysUntil(notificationId, model.AsDateTime().GetValueOrDefault()));

            if (workingDaysUntilShipment < 4)
            {
                // todo: add model to tempdata?
                return RedirectToAction("ThreeWorkingDaysWarning", "Create");
            }

            var hasExceededTotalQuantity =
                await mediator.SendAsync(new HasExceededConsentedQuantity(notificationId, Convert.ToDecimal(model.Quantity) * model.NumberToCreate.Value, model.Units.Value));

            if (hasExceededTotalQuantity)
            {
                ModelState.AddModelError("Quantity", CreateMovementsViewModelResources.HasExceededTotalQuantity);
            }

            // todo: check total number of shipments not exceeded

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var newMovementDetails = new NewMovementDetails
            {
                Quantity = Convert.ToDecimal(model.Quantity),
                Units = model.Units.Value,
                PackagingTypes = model.SelectedValues
            };

            // todo: create multiple
            var newMovementId = await mediator.SendAsync(new CreateMovementAndDetails(notificationId, model.AsDateTime().Value, newMovementDetails));

            return RedirectToAction("Download", "Create", new { id = newMovementId });
        }

        private ActionResult GetRuleErrorView(MovementRulesSummary ruleSummary)
        {
            if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.TotalShipmentsReached && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("TotalMovementsReached");
            }
            else if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.TotalIntendedQuantityReached && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("TotalIntendedQuantityReached");
            }
            else if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.TotalIntendedQuantityExceeded && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("TotalIntendedQuantityExceeded");
            }
            else if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.HasApprovedFinancialGuarantee && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("NoApprovedFinancialGuarantee");
            }
            else if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.ActiveLoadsReached && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("TotalActiveLoadsReached");
            }
            else if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.ConsentPeriodExpired && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("ConsentPeriodExpired");
            }
            else if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.ConsentExpiresInFourWorkingDays && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("ConsentExpiresInFourWorkingDays");
            }
            else if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.ConsentExpiresInThreeOrLessWorkingDays && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("ConsentExpiresInThreeOrLessWorkingDays");
            }
            else if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.ConsentWithdrawn && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("ConsentWithdrawn");
            }
            else if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.FileClosed && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("FileClosed");
            }

            throw new InvalidOperationException("Unknown rule view");
        }

        [HttpGet]
        public ActionResult ThreeWorkingDaysWarning(Guid notificationId)
        {
            // todo: read model from tempdata?
            var model = new ThreeWorkingDaysWarningViewModel();

            return View("ThreeWorkingDays", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThreeWorkingDaysWarning(Guid notificationId, ThreeWorkingDaysWarningViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ThreeWorkingDays", model);
            }

            if (model.Selection == ThreeWorkingDaysSelection.ChangeDate)
            {
                return RedirectToAction("Index");
            }

            // todo: save movements, redirect to 'what to do next'
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Download(Guid notificationId, Guid id)
        {
            var model = new DownloadViewModel { MovementId = id };

            ViewBag.MovementNumber = await mediator.SendAsync(new GetMovementNumberByMovementId(id));

            return View(model);
        }

        [HttpGet]
        public ActionResult TotalMovementsReached(Guid notificationId)
        {
            return View();
        }
        
        [HttpGet]
        public ActionResult TotalActiveLoadsReached(Guid notificationId)
        {
            return View();
        }
        
        [HttpGet]
        public ActionResult TotalIntendedQuantityReached(Guid notificationId)
        {
            return View();
        }

        [HttpGet]
        public ActionResult TotalIntendedQuantityExceeded(Guid notificationId)
        {
            return View();
        }

        [HttpGet]
        public ActionResult ConsentPeriodExpired(Guid notificationId)
        {
            return View();
        }

        [HttpGet]
        public ActionResult ConsentExpiresInFourWorkingDays(Guid notificationId)
        {
            return View();
        }

        [HttpGet]
        public ActionResult ConsentExpiresInThreeOrLessWorkingDays(Guid notificationId)
        {
            return View();
        }

        [HttpGet]
        public ActionResult ConsentWithdrawn(Guid notificationId)
        {
            return View();
        }

        [HttpGet]
        public ActionResult FileClosed(Guid notificationId)
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> NoApprovedFinancialGuarantee(Guid notificationId)
        {
            var competentAuthority =
                await mediator.SendAsync(new GetUnitedKingdomCompetentAuthorityByNotificationId(notificationId));

            return View(competentAuthority.AsUKCompetantAuthority());
        }
    }
}