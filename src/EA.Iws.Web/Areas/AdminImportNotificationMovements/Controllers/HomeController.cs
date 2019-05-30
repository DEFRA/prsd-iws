﻿namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.ImportNotificationAssessment;
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

            var model = new MovementSummaryViewModel(movementData, tableData);

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
    }
}