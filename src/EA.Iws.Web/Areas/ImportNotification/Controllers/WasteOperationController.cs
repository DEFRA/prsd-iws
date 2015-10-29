namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.OperationCodes;
    using Core.Shared;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using ViewModels.WasteOperation;
    using Web.ViewModels.Shared;

    public class WasteOperationController : Controller
    {
        private readonly IMediator mediator;

        public WasteOperationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var details = await mediator.SendAsync(new GetNotificationDetails(id));

            var model = new WasteOperationViewModel(details);
            if (details.NotificationType == NotificationType.Recovery)
            {
                model.Codes = CheckBoxCollectionViewModel.CreateFromEnum<RecoveryCode>();
            }
            else
            {
                model.Codes = CheckBoxCollectionViewModel.CreateFromEnum<DisposalCode>();
            }

            return View(model);
        }
    }
}