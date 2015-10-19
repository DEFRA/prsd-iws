namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.PackagingType;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.PackagingType;
    using ViewModels.PackagingTypes;
    using Web.ViewModels.Shared;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class PackagingTypesController : Controller
    {
        private readonly IMediator mediator;

        public PackagingTypesController(IMediator mediator)
        {
            this.mediator = mediator;
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
                    ModelState.AddModelError(string.Empty, "Please select at least one option");
                    return View(model);
                }

                await mediator.SendAsync(new SetPackagingInfoForNotification(selectedPackagingTypes, model.NotificationId,
                        model.OtherDescription));

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