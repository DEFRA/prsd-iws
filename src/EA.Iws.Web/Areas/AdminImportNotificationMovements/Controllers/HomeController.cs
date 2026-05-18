namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.ImportNotificationAssessment;
    using DocumentFormat.OpenXml.EMMA;
    using EA.Iws.Core.ImportNotificationMovements;
    using EA.Iws.Core.Movement;
    using EA.Iws.Core.NotificationAssessment;
    using EA.Iws.Core.WasteType;
    using EA.Iws.Requests.Admin.NotificationAssessment;
    using EA.Iws.Requests.ImportNotificationAssessment;
    using EA.Iws.Requests.NotificationMovements;
    using EA.Iws.Requests.WasteType;
    using EA.Prsd.Core.Helpers;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement.Capture;
    using Requests.ImportMovement.Delete;
    using Requests.ImportNotification;
    using Requests.ImportNotificationMovements;
    using ViewModels.Home;
    using Web.ViewModels.Shared;

    [AuthorizeActivity(typeof(GetImportMovementsSummary))]
    [AuthorizeActivity(typeof(GetImportMovementsSummaryTable))]
    public class HomeController : Controller
    {
        private readonly IMediator mediator;
        private readonly AuthorizationService authorizationService;
        //For Cancel prenotification
        private const string SubmittedMovementListKey = "SubmittedMovementListKey";
        private const string AddedCancellableMovementsListKey = "AddedCancellableMovementsListKey";

        public string PreNotificationWarnings { get; set; }
        public string EarlyShipmentWarnings { get; set; }
        public string ConsentedDateWarnings { get; set; }
        public List<MovementsSummaryTableViewModel> TableData { get; set; }
        public List<NotificationAssessmentDecision> Decisions { get; set; }

        public HomeController(IMediator mediator, AuthorizationService authorizationService)
        {
            this.mediator = mediator;
            this.authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, int page = 1)
        {
            TempData[SubmittedMovementListKey] = null;
            TempData[AddedCancellableMovementsListKey] = null;

            var movementData = await mediator.SendAsync(new GetImportMovementsSummary(id));
            var tableData = await mediator.SendAsync(new GetImportMovementsSummaryTable(id, page));
            var canDeleteMovement = await authorizationService.AuthorizeActivity(typeof(DeleteMovement));
            var keyDates = await mediator.SendAsync(new GetKeyDates(id));

            TableData = tableData.TableData.OrderByDescending(d => d.Number).Select(d => new MovementsSummaryTableViewModel(d)).ToList();
            Decisions = new List<NotificationAssessmentDecision>(
                keyDates.DecisionHistory.Where(d => d.Status == EA.Iws.Core.NotificationAssessment.NotificationStatus.Consented));

            PreNotificationWarnings = GetPreNotificationWarnings(TableData);
            EarlyShipmentWarnings = GetEarlyShipmentWarnings(TableData);
            ConsentedDateWarnings = GetConsentedDateExceededWarnings(Decisions, TableData);

            var model = new MovementSummaryViewModel(movementData, tableData, PreNotificationWarnings, EarlyShipmentWarnings, ConsentedDateWarnings);

            model.CanDeleteMovement = canDeleteMovement && movementData.NotificationStatus != ImportNotificationStatus.FileClosed;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Search(Guid id, int? shipmentNumber, int? status, int page = 1)
        {
            if (!shipmentNumber.HasValue || shipmentNumber.Value <= 0)
            {
                return RedirectToAction("Index");
            }

            WasteTypeData wasteTypeData = null;

            try
            {
                wasteTypeData = await mediator.SendAsync(new GetWasteType(id));
            }
            catch
            {
                // This is a test to make sure that if the waste type data cannot be retrieved, the user can still save the shipment data.
                // The waste type data is only used to validate the total shipments field, so if it cannot be retrieved, we will not perform that validation.
            }

            if (shipmentNumber != null && wasteTypeData == null)
            {
                if ((shipmentNumber > 1) &&
                    (wasteTypeData.WasteCategoryType == WasteCategoryType.Singleship || 
                     wasteTypeData.WasteCategoryType == WasteCategoryType.Platformrig))
                {
                    ModelState.AddModelError("ShipmentNumber", "Only one shipment is allowed for Waste Category Type: " + EnumHelper.GetDisplayName<WasteCategoryType>((WasteCategoryType)wasteTypeData.WasteCategoryType));
                }
            }

            if (!ModelState.IsValid)
            {
                var movementData = await mediator.SendAsync(new GetImportMovementsSummary(id));
                var tableData = await mediator.SendAsync(new GetImportMovementsSummaryTable(id, page));
                var canDeleteMovement = await authorizationService.AuthorizeActivity(typeof(DeleteMovement));
                var keyDates = await mediator.SendAsync(new GetKeyDates(id));

                TableData = tableData.TableData.OrderByDescending(d => d.Number).Select(d => new MovementsSummaryTableViewModel(d)).ToList();
                Decisions = new List<NotificationAssessmentDecision>(
                    keyDates.DecisionHistory.Where(d => d.Status == EA.Iws.Core.NotificationAssessment.NotificationStatus.Consented));

                PreNotificationWarnings = GetPreNotificationWarnings(TableData);
                EarlyShipmentWarnings = GetEarlyShipmentWarnings(TableData);
                ConsentedDateWarnings = GetConsentedDateExceededWarnings(Decisions, TableData);

                var model = new MovementSummaryViewModel(movementData, tableData, PreNotificationWarnings, EarlyShipmentWarnings, ConsentedDateWarnings);

                model.CanDeleteMovement = canDeleteMovement && movementData.NotificationStatus != ImportNotificationStatus.FileClosed;
                
                return View(model);
            }

            var movementId = await mediator.SendAsync(new GetImportMovementIdIfExists(id, shipmentNumber.Value));
            if (movementId.HasValue)
            {
                return RedirectToAction("Edit", "Capture", new { movementId });
            }
            else
            {
                return RedirectToAction("Create", "Capture", new { shipmentNumber });
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult NotificationSwitcher(Guid id)
        {
            var response = Task.Run(() => mediator.SendAsync(new GetNotificationDetails(id))).Result;

            return PartialView("_NotificationSwitcher", new NotificationSwitcherViewModel(response.NotificationNumber));
        }

        private string GetPreNotificationWarnings(List<MovementsSummaryTableViewModel> tableData)
        {
            var warnings = new StringBuilder();

            foreach (var row in tableData)
            {
                DateTime? preNotDate = row.PreNotification;
                DateTime? shipDate = row.ShipmentDate;

                if (preNotDate.HasValue && shipDate.HasValue)
                {
                    var difference = (shipDate.Value.Date - preNotDate.Value.Date).Days;

                    if (difference < 3)
                    {
                        warnings.Append(", " + row.Number.ToString());
                    }
                }
            }

            if (warnings.Length == 0)
            {
                return string.Empty;
            }
            else
            {
                return " for shipments: " + warnings.ToString().Remove(0, 2);
            }
        }

        private string GetEarlyShipmentWarnings(List<MovementsSummaryTableViewModel> tableData)
        {
            var warnings = new StringBuilder();

            foreach (var row in tableData)
            {
                DateTime? shipDate = row.ShipmentDate;
                DateTime? receivedDate = row.Received;

                if (shipDate.HasValue && receivedDate.HasValue)
                {
                    if (DateTime.Compare((DateTime)shipDate, (DateTime)receivedDate) > 0)
                    {
                        warnings.Append(", " + row.Number.ToString());
                    }
                }
            }

            if (warnings.Length == 0)
            {
                return string.Empty;
            }
            else
            {
                return " for shipments: " + warnings.ToString().Remove(0, 2);
            }
        }

        private string GetConsentedDateExceededWarnings(List<NotificationAssessmentDecision> decisions, List<MovementsSummaryTableViewModel> tableData)
        {
            var warnings = new StringBuilder();

            if (decisions != null && decisions.Any())
            {
                var mostRecentConsentedDecision = decisions.OrderByDescending(d => d.Date).FirstOrDefault(d => d.Status == EA.Iws.Core.NotificationAssessment.NotificationStatus.Consented);
                var mostRecentConsentedDate = mostRecentConsentedDecision.ConsentedTo;

                foreach (var row in tableData)
                {
                    DateTime? shipDate = row.ShipmentDate;

                    if (shipDate.HasValue)
                    {
                        if (DateTime.Compare((DateTime)shipDate, (DateTime)mostRecentConsentedDate) > 0)
                        {
                            warnings.Append(", " + row.Number.ToString());
                        }
                    }
                }
            }

            if (warnings.Length == 0)
            {
                return string.Empty;
            }
            else
            {
                return " for shipments: " + warnings.ToString().Remove(0, 2);
            }
        }
    }
}