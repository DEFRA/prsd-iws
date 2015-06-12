namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Prsd.Core.Extensions;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.WasteType;
    using ViewModels.Shared;
    using ViewModels.WasteType;

    [Authorize]
    public class WasteTypeController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public WasteTypeController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public ActionResult ChemicalComposition(Guid id)
        {
            var model = new ChemicalCompositionViewModel()
            {
                NotificationId = id,
                ChemicalCompositionType = RadioButtonStringCollectionViewModel.CreateFromEnum<ChemicalCompositionType>(),
                WasteCompositions = new List<WasteCompositionData> { new WasteCompositionData() }
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChemicalComposition(ChemicalCompositionViewModel model, string submitButton)
        {
            switch (submitButton)
            {
                case "Continue":
                    return (await ChemicalCompositionSave(model));
                case "AddMore":
                    return (ChemicalCompositionAddMore(model));
            }
            return View();
        }

        private ActionResult ChemicalCompositionAddMore(ChemicalCompositionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.WasteCompositions != null)
            {
                bool isValid = ValidateWasteComposition(model.WasteCompositions);
                if (isValid == false)
                {
                    return View(model);
                }
            }

            if (model.WasteCompositions != null)
            {
                model.WasteCompositions.Add(new WasteCompositionData());
            }
            return View("ChemicalComposition", model);
        }

        private async Task<ActionResult> ChemicalCompositionSave(ChemicalCompositionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = apiClient())
            {
                var chemicalCompositionType = model.ChemicalCompositionType.SelectedValue.GetValueFromDisplayName<ChemicalCompositionType>();

                //To remove blank Waste Composition item from List, if any
                if (chemicalCompositionType == ChemicalCompositionType.RDF ||
                    chemicalCompositionType == ChemicalCompositionType.SRF)
                {
                    if (String.IsNullOrWhiteSpace(model.WasteCompositions[model.WasteCompositions.Count - 1].Constituent))
                    {
                        model.WasteCompositions.RemoveAt(model.WasteCompositions.Count - 1);
                    }
                    if (model.WasteCompositions.Count == 0)
                    {
                        model.WasteCompositions.Add(new WasteCompositionData());
                        ModelState.AddModelError("CompositionValidationError", "Waste composition is required.");
                        return View(model);
                    }
                    if (false == ValidateWasteComposition(model.WasteCompositions))
                    {
                        return View(model);
                    }

                    foreach (var wasteCompositionItem in model.WasteCompositions)
                    {
                        wasteCompositionItem.MinConcentration = decimal.Round(wasteCompositionItem.MinConcentration, 2,
                            MidpointRounding.AwayFromZero);
                        wasteCompositionItem.MaxConcentration = decimal.Round(wasteCompositionItem.MaxConcentration, 2,
                            MidpointRounding.AwayFromZero);
                    }
                }

                await client.SendAsync(User.GetAccessToken(),
                        new CreateWasteType(model.NotificationId, chemicalCompositionType, model.OtherCompositionName, model.Description,
                            model.WasteCompositions));
            }
            return RedirectToAction("WasteGenerationProcess", new { id = model.NotificationId });
        }

        private bool ValidateWasteComposition(List<WasteCompositionData> wasteCompositions)
        {
            foreach (var wasteCompositionItem in wasteCompositions)
            {
                bool isValid = true;
                if (String.IsNullOrWhiteSpace(wasteCompositionItem.Constituent))
                {
                    isValid = false;
                    ModelState.AddModelError("ConstituentValidationError", "Constituent is required.");
                }
                if (wasteCompositionItem.MinConcentration < 0 || wasteCompositionItem.MinConcentration > 100)
                {
                    isValid = false;
                    ModelState.AddModelError("MinConcentrationValidationError", "Min concentration should be in range from 0 to 100.");
                }
                if (wasteCompositionItem.MaxConcentration < 0 || wasteCompositionItem.MaxConcentration > 100)
                {
                    isValid = false;
                    ModelState.AddModelError("MaxConcentrationValidationError", "Max concentration should be in range from 0 to 100.");
                }
                if (wasteCompositionItem.MinConcentration > wasteCompositionItem.MaxConcentration)
                {
                    isValid = false;
                    ModelState.AddModelError("MinMaxValidationError", "Min concentration can not be greater than or equal to max concentration.");
                }
                if (isValid == false)
                {
                    return false;
                }
            }
            return true;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult WasteGenerationProcess(Guid id)
        {
            var model = new WasteGenerationProcessViewModel
            {
                NotificationId = id,
                ProcessDescription = string.Empty
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> WasteGenerationProcess(WasteGenerationProcessViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (var client = apiClient())
            {
                try
                {
                    await client.SendAsync(User.GetAccessToken(), new SetWasteGenerationProcess(model.ProcessDescription, model.NotificationId, model.IsDocumentAttached));
                    return RedirectToAction("PhysicalCharacteristics", new { id = model.NotificationId });
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
        public ActionResult PhysicalCharacteristics(Guid id)
        {
            var physicalCharacteristics = CheckBoxCollectionViewModel.CreateFromEnum<PhysicalCharacteristicType>();
            physicalCharacteristics.ShowEnumValue = true;

            //We need to exclude 'other' as this will be handled separately
            physicalCharacteristics.PossibleValues = physicalCharacteristics.PossibleValues.Where(p => (PhysicalCharacteristicType)Convert.ToInt32(p.Value) != PhysicalCharacteristicType.Other).ToList();

            var model = new PhysicalCharacteristicsViewModel
            {
                PhysicalCharacteristics = physicalCharacteristics,
                NotificationId = id
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PhysicalCharacteristics(PhysicalCharacteristicsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = apiClient())
            {
                try
                {
                    var selectedPackagingTypes = model.PhysicalCharacteristics.PossibleValues.Where(p => p.Selected).Select(p => (PhysicalCharacteristicType)(Convert.ToInt32(p.Value))).ToList();
                    
                    if (model.OtherSelected)
                    {
                        selectedPackagingTypes.Add(PhysicalCharacteristicType.Other);
                    }

                    await client.SendAsync(User.GetAccessToken(), new SetPhysicalCharacteristics(selectedPackagingTypes, model.NotificationId, model.OtherDescription));
                    return RedirectToAction("Index", "Home");
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
        public async Task<ActionResult> WasteCode(Guid id)
        {
            using (var client = apiClient())
            {
                var model = new WasteCodeViewModel
                {
                    NotificationId = id,
                    WasteCodes = await GetWasteCodes()
                };

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> WasteCode(WasteCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.WasteCodes = await GetWasteCodes();
                return View(model);
            }

            using (var client = apiClient())
            {
                try
                {
                    await client.SendAsync(User.GetAccessToken(), new SetWasteCode(new Guid(model.SelectedWasteCode), model.NotificationId));
                    return RedirectToAction("Index", "Home");
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

        private async Task<IEnumerable<WasteCodeData>> GetWasteCodes()
        {
            using (var client = apiClient())
            {
                return await client.SendAsync(User.GetAccessToken(), new GetWasteCodes());
            }
        }
    }
}