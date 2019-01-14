namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using Core.Notification.Audit;
    using Core.WasteType;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.WasteType;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.PhysicalCharacteristics;
    using Web.ViewModels.Shared;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class PhysicalCharacteristicsController : Controller
    {
        private readonly IMediator mediator;
        private readonly IAuditService auditService;

        public PhysicalCharacteristicsController(IMediator mediator, IAuditService auditService)
        {
            this.mediator = mediator;
            this.auditService = auditService;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, bool? backToOverview = null)
        {
            var physicalCharacteristics = CheckBoxCollectionViewModel.CreateFromEnum<PhysicalCharacteristicType>();
            physicalCharacteristics.ShowEnumValue = true;

            //We need to exclude 'other' as this will be handled separately
            physicalCharacteristics.PossibleValues =
                physicalCharacteristics.PossibleValues.Where(
                    p => (PhysicalCharacteristicType)Convert.ToInt32(p.Value) != PhysicalCharacteristicType.Other)
                    .ToList();

            var model = new PhysicalCharacteristicsViewModel
            {
                PhysicalCharacteristics = physicalCharacteristics,
                NotificationId = id
            };

            var physicalCharacteristicsData =
                await mediator.SendAsync(new GetPhysicalCharacteristics(id));

            if (physicalCharacteristicsData != null)
            {
                model.PhysicalCharacteristics.SetSelectedValues(physicalCharacteristicsData.PhysicalCharacteristics);
                if (!string.IsNullOrWhiteSpace(physicalCharacteristicsData.OtherDescription))
                {
                    model.OtherSelected = true;
                    model.OtherDescription = physicalCharacteristicsData.OtherDescription;
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(PhysicalCharacteristicsViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var selectedPackagingTypes =
                    model.PhysicalCharacteristics.PossibleValues.Where(p => p.Selected)
                        .Select(p => (PhysicalCharacteristicType)(Convert.ToInt32(p.Value)))
                        .ToList();

                if (model.OtherSelected)
                {
                    selectedPackagingTypes.Add(PhysicalCharacteristicType.Other);
                }

                var existingPhysicalCharacteristicsData = await mediator.SendAsync(new GetPhysicalCharacteristics(model.NotificationId));

                await
                    mediator.SendAsync(new SetPhysicalCharacteristics(selectedPackagingTypes, model.NotificationId,
                            model.OtherDescription));

                await this.auditService.AddAuditEntry(this.mediator,
                   model.NotificationId,
                   User.GetUserId(),
                   existingPhysicalCharacteristicsData.PhysicalCharacteristics.Count == 0 ? NotificationAuditType.Create : NotificationAuditType.Update,
                   "Physical characteristics");

                if (backToOverview.GetValueOrDefault())
                {
                    return RedirectToAction("Index", "Home", new { id = model.NotificationId });
                }

                return RedirectToAction("Index", "BaselOecdCode", new { id = model.NotificationId });
            }
            catch (ApiBadRequestException ex)
            {
                this.HandleBadRequest(ex);
                if (ModelState.IsValid)
                {
                    throw;
                }
            }
            return View(model);
        }
    }
}