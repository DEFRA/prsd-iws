namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using Core.Notification.Audit;
    using Core.PackagingType;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.PackagingType;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.PackagingTypes;
    using Views.PackagingTypes;
    using Web.ViewModels.Shared;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class PackagingTypesController : Controller
    {
        private readonly IMediator mediator;
        private readonly IAuditService auditService;

        public PackagingTypesController(IMediator mediator, IAuditService auditService)
        {
            this.mediator = mediator;
            this.auditService = auditService;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, bool? backToOverview = null)
        {
            var packagingTypes = CheckBoxCollectionViewModel.CreateFromEnum<PackagingType>();
            packagingTypes.ShowEnumValue = true;

            //We need to exclude 'other' as this will be handled separately
            packagingTypes.PossibleValues =
                packagingTypes.PossibleValues.Where(p => (PackagingType)Convert.ToInt32(p.Value) != PackagingType.Other)
                    .ToList();

            var model = new PackagingTypesViewModel
            {
                PackagingTypes = packagingTypes,
                NotificationId = id
            };

            var packagingData =
                await mediator.SendAsync(new GetPackagingInfoForNotification(id));

            if (packagingData != null)
            {
                model.PackagingTypes.SetSelectedValues(packagingData.PackagingTypes);
                if (!string.IsNullOrWhiteSpace(packagingData.OtherDescription))
                {
                    model.OtherSelected = true;
                    model.OtherDescription = packagingData.OtherDescription;
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(PackagingTypesViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var selectedPackagingTypes =
                    model.PackagingTypes.PossibleValues.Where(p => p.Selected)
                        .Select(p => (PackagingType)(Convert.ToInt32(p.Value)))
                        .ToList();

                if (model.OtherSelected)
                {
                    selectedPackagingTypes.Add(PackagingType.Other);
                }

                if (!selectedPackagingTypes.Any())
                {
                    ModelState.AddModelError(string.Empty, PackagingTypesResources.ChoosePackagingType);
                    return View(model);
                }

                var existingPackagingData = await mediator.SendAsync(new GetPackagingInfoForNotification(model.NotificationId));

                await mediator.SendAsync(new SetPackagingInfoForNotification(selectedPackagingTypes, model.NotificationId,
                        model.OtherDescription));

                await this.auditService.AddAuditEntry(this.mediator,
                    model.NotificationId,
                    User.GetUserId(),
                    existingPackagingData.PackagingTypes.Count == 0 ? NotificationAuditType.Added : NotificationAuditType.Updated,
                    NotificationAuditScreenType.PackagingTypes);

                if (backToOverview.GetValueOrDefault())
                {
                    return RedirectToAction("Index", "Home", new { id = model.NotificationId });
                }
                else
                {
                    return RedirectToAction("Index", "SpecialHandling", new { id = model.NotificationId });
                }
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