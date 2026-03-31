namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.ImportNotificationAssessment;
    using EA.Iws.Core.ImportNotificationMovements;
    using EA.Iws.Core.NotificationAssessment;
    using EA.Iws.Requests.ImportNotificationAssessment;
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
        public async Task<ActionResult> Search(Guid id, int? shipmentNumber)
        {
            if (!shipmentNumber.HasValue || shipmentNumber.Value <= 0)
            {
                return RedirectToAction("Index");
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
                var mostRecentConsentedDate = mostRecentConsentedDecision.Date;

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