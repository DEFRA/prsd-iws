namespace EA.Iws.Web.Areas.Movement.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Movement;
    using ViewModels;
    using Web.ViewModels.Shared;
    using PackagingType = Core.PackagingType.PackagingType;

    public class PackagingTypesController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public PackagingTypesController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid movementId)
        {
            using (var client = apiClient())
            {
                var availablePackagingInfo = await client.SendAsync(User.GetAccessToken(),
                    new GetPackagingDataValidForMovement(movementId));

                var selectedPackagingInfo = await client.SendAsync(User.GetAccessToken(),
                    new GetPackagingDataForMovement(movementId));

                var checkBoxCollection = new CheckBoxCollectionViewModel();
                
                checkBoxCollection.PossibleValues = new List<SelectListItem>();
                foreach (var packagingType in availablePackagingInfo.PackagingTypes.OrderBy(pt => pt))
                {
                    var item = new SelectListItem();

                    if (packagingType == PackagingType.Other)
                    {
                        item.Text = string.Format("Other - {0}", availablePackagingInfo.OtherDescription);
                        item.Value = ((int)packagingType).ToString();
                    }
                    else
                    {
                        item.Text = EA.Prsd.Core.Helpers.EnumHelper.GetDisplayName(packagingType);
                        item.Value = ((int)packagingType).ToString();
                    }

                    checkBoxCollection.PossibleValues.Add(item);
                }

                if (selectedPackagingInfo.PackagingTypes.Any())
                {
                    checkBoxCollection.SetSelectedValues(selectedPackagingInfo.PackagingTypes);
                }

                checkBoxCollection.ShowEnumValue = true;

                var model = new PackagingTypesViewModel()
                {
                    PackagingTypes = checkBoxCollection,
                    MovementId = movementId
                };

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid movementId, PackagingTypesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = apiClient())
            {
                try
                {
                    var selectedPackagingTypes = model.PackagingTypes.PossibleValues
                                                    .Where(p => p.Selected)
                                                    .Select(p => (PackagingType)Convert.ToInt32(p.Value))
                                                    .ToList();

                    if (!selectedPackagingTypes.Any())
                    {
                        ModelState.AddModelError(string.Empty, "Please select at least one option");
                        return View(model);
                    }

                    await client.SendAsync(User.GetAccessToken(), new SetPackagingDataForMovement(model.MovementId, selectedPackagingTypes));

                    return RedirectToAction("Index", "NumberOfPackages", new { movementId });
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