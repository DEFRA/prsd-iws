namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Core.WasteType;
    using Infrastructure;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.WasteType;
    using ViewModels.PhysicalCharacteristics;
    using ViewModels.WasteType;
    using Web.ViewModels.Shared;

    public class PhysicalCharacteristicsController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public PhysicalCharacteristicsController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
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

            using (var client = apiClient())
            {
                var physicalCharacteristicsData =
                    await client.SendAsync(User.GetAccessToken(), new GetPhysicalCharacteristics(id));

                if (physicalCharacteristicsData != null)
                {
                    model.PhysicalCharacteristics.SetSelectedValues(physicalCharacteristicsData.PhysicalCharacteristics);
                    if (!string.IsNullOrWhiteSpace(physicalCharacteristicsData.OtherDescription))
                    {
                        model.OtherSelected = true;
                        model.OtherDescription = physicalCharacteristicsData.OtherDescription;
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(PhysicalCharacteristicsViewModel model)
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
                        model.PhysicalCharacteristics.PossibleValues.Where(p => p.Selected)
                            .Select(p => (PhysicalCharacteristicType)(Convert.ToInt32(p.Value)))
                            .ToList();

                    if (model.OtherSelected)
                    {
                        selectedPackagingTypes.Add(PhysicalCharacteristicType.Other);
                    }

                    await
                        client.SendAsync(User.GetAccessToken(),
                            new SetPhysicalCharacteristics(selectedPackagingTypes, model.NotificationId,
                                model.OtherDescription));
                    return RedirectToAction("WasteCode", "WasteCodes", new { id = model.NotificationId });
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