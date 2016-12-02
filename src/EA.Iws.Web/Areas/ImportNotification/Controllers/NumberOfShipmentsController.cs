namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Authorization.Permissions;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using ViewModels.NumberOfShipments;

    [AuthorizeActivity(ImportNotificationPermissions.CanChangeNumberOfShipmentsOnImportNotification)]
    public class NumberOfShipmentsController : Controller
    {
        private readonly IMediator mediator;

        public NumberOfShipmentsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Guid id, IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("Confirm", model);
        }

        [HttpGet]
        public async Task<ActionResult> Confirm(Guid id, IndexViewModel model)
        {
            var data = await mediator.SendAsync(new GetChangeNumberOfShipmentConfrimationData(id, model.Number.GetValueOrDefault()));
            var confirmModel = new ConfirmViewModel(data);
            confirmModel.NewNumberOfShipments = model.Number.GetValueOrDefault();

            return View(confirmModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(ConfirmViewModel model)
        {
            mediator.SendAsync(new SetNewNumberOfShipments(model.NotificationId, model.OldNumberOfShipments, model.NewNumberOfShipments));

            return RedirectToAction("Index", "Home");
        }
    }
}