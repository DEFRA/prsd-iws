namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.PackagingType;
    using ViewModels.PackagingTypes;
    using Web.ViewModels.Shared;

    [Authorize]
    public class PackagingTypesController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public PackagingTypesController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Add(Guid id)
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

            using (var client = apiClient())
            {
                var packagingData =
                    await client.SendAsync(User.GetAccessToken(), new GetPackagingTypesForNotification(id));

                if (packagingData.PackagingTypes.Count > 0)
                {
                    return RedirectToAction("Edit", new { id });
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(PackagingTypesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = apiClient())
            {
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

                    await client.SendAsync(User.GetAccessToken(),
                        new SetPackagingTypeOnShipmentInfo(selectedPackagingTypes, model.NotificationId,
                            model.OtherDescription));

                    return RedirectToAction("Add", "SpecialHandling", new { id = model.NotificationId });
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

        [HttpGet]
        public async Task<ActionResult> Edit(Guid id)
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

            using (var client = apiClient())
            {
                var packagingData =
                    await client.SendAsync(User.GetAccessToken(), new GetPackagingTypesForNotification(id));

                if (packagingData == null)
                {
                    return RedirectToAction("Add", new { id });
                }

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
        public async Task<ActionResult> Edit(PackagingTypesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = apiClient())
            {
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

                    await client.SendAsync(User.GetAccessToken(),
                        new SetPackagingTypeOnShipmentInfo(selectedPackagingTypes, model.NotificationId,
                            model.OtherDescription));

                    return RedirectToAction("Index", "Home", new { id = model.NotificationId });
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
}