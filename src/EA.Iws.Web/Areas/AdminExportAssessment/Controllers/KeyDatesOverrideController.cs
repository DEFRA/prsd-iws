namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Admin.KeyDates;
    using Core.Authorization.Permissions;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Admin.KeyDates;
    using ViewModels.KeyDatesOverride;

    [AuthorizeActivity(UserAdministrationPermissions.CanOverrideKeyDates)]
    public class KeyDatesOverrideController : Controller
    {
        private readonly IMediator mediator;

        public KeyDatesOverrideController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var dates = await mediator.SendAsync(new GetExportKeyDatesOverrideData(id));
            var model = new IndexViewModel(dates);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var data = new KeyDatesOverrideData
            {
                AcknowledgedDate = model.AcknowledgedDate.AsDateTime(),
                CommencementDate = model.CommencementDate.AsDateTime(),
                CompleteDate = model.CompleteDate.AsDateTime(),
                ConsentedDate = model.ConsentedDate.AsDateTime(),
                ConsentValidFromDate = model.ConsentValidFromDate.AsDateTime(),
                ConsentValidToDate = model.ConsentValidToDate.AsDateTime(),
                NotificationId = id,
                NotificationReceivedDate = model.NotificationReceivedDate.AsDateTime(),
                ObjectedDate = model.ObjectedDate.AsDateTime(),
                WithdrawnDate = model.WithdrawnDate.AsDateTime(),
                TransmittedDate = model.TransmittedDate.AsDateTime()
            };

            await mediator.SendAsync(new SetExportKeyDatesOverride(data));

            return RedirectToAction("Index", "KeyDates");
        }
    }
}