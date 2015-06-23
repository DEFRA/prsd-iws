namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
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
    using ViewModels.WasteType;
    using Web.ViewModels.Shared;

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
                    return RedirectToAction("WasteCode", new { id = model.NotificationId });
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
            var model = new WasteCodeViewModel
            {
                NotificationId = id,
            };

            await InitializeWasteCodeViewModel(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> WasteCode(WasteCodeViewModel model, string command)
        {
            if (!ModelState.IsValid)
            {
                await InitializeWasteCodeViewModel(model);
                return View(model);
            }

            if (command.Equals("add"))
            {
                await InitializeWasteCodeViewModel(model);
                var codeToAdd = model.EwcCodes.Single(c => c.Id.ToString() == model.SelectedEwcCode);
                if (model.SelectedEwcCodes.All(c => c.Id != codeToAdd.Id))
                {
                   model.SelectedEwcCodes.Add(codeToAdd); 
                }
                return View(model);
            }

            using (var client = apiClient())
            {
                try
                {
                    await client.SendAsync(User.GetAccessToken(), new SetBaselOrOecdWasteCode(new Guid(model.SelectedWasteCode), model.NotificationId));
                    await client.SendAsync(User.GetAccessToken(), new SetEwcCodes(model.SelectedEwcCodes, model.NotificationId));
                    return RedirectToAction("OtherWasteCodes", new { id = model.NotificationId });
                }
                catch (ApiBadRequestException ex)
                {
                    this.HandleBadRequest(ex);
                    if (ModelState.IsValid)
                    {
                        throw;
                    }
                }
                await InitializeWasteCodeViewModel(model);
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult OtherWasteCodes(Guid id)
        {
            var model = new OtherWasteCodesViewModel
            {
                NotificationId = id,
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> OtherWasteCodes(OtherWasteCodesViewModel model, string command)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (var client = apiClient())
            {
                try
                {
                    List<WasteCodeData> wasteCodeData = new List<WasteCodeData>();
                    if (!string.IsNullOrEmpty(model.ExportNationalCode))
                    {
                        wasteCodeData.Add(new WasteCodeData { OptionalCode = model.ExportNationalCode, CodeType = CodeType.ExportCode, OptionalDescription = "National code in country of export" });
                    }
                    if (!string.IsNullOrEmpty(model.ImportNationalCode))
                    {
                        wasteCodeData.Add(new WasteCodeData { OptionalCode = model.ImportNationalCode, CodeType = CodeType.ImportCode, OptionalDescription = "National code in country of import" });
                    }
                    if (!string.IsNullOrEmpty(model.OtherCode))
                    {
                        wasteCodeData.Add(new WasteCodeData { OptionalCode = model.OtherCode, CodeType = CodeType.OtherCode, OptionalDescription = "Other code" });
                    }
                    await client.SendAsync(User.GetAccessToken(), new SetOptionalWasteCodes(model.NotificationId, wasteCodeData));
                    return RedirectToAction("UnNumber", new { id = model.NotificationId });
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
        public async Task<ActionResult> UnNumber(Guid id)
        {
            var model = new UnNumberViewModel
            {
                NotificationId = id,
            };
            await InitializeUnNumberViewModel(model);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UnNumber(UnNumberViewModel model, string command)
        {
            if (!ModelState.IsValid)
            {
                await InitializeUnNumberViewModel(model);
                return View(model);
            }
            if (command.Equals("add"))
            {
                await InitializeUnNumberViewModel(model);
                var codeToAdd = model.UnCodes.Single(c => c.Id.ToString() == model.SelectedUnCode);
                if (model.SelectedUnCodes.All(c => c.Id != codeToAdd.Id))
                {
                    model.SelectedUnCodes.Add(codeToAdd);
                }
                return View(model);
            }
            if (command.Equals("addCustomCode"))
            {
                await InitializeUnNumberViewModel(model);
                if (model.CustomCodes == null)
                {
                    model.CustomCodes = new List<string>();
                }
                model.CustomCodes.Add(model.SelectedCustomCode);
                return View(model);
            }
            using (var client = apiClient())
            {
                try
                {
                    if (model.CustomCodes != null)
                    {
                        List<WasteCodeData> wasteCodeData = new List<WasteCodeData>();
                        foreach (var customCode in model.CustomCodes)
                        {
                            wasteCodeData.Add(new WasteCodeData { OptionalCode = customCode, CodeType = CodeType.CustomCode, OptionalDescription = "Custom Code" });
                        }
                        await client.SendAsync(User.GetAccessToken(), new SetOptionalWasteCodes(model.NotificationId, wasteCodeData));
                    }
                    await client.SendAsync(User.GetAccessToken(), new SetEwcCodes(model.SelectedUnCodes, model.NotificationId));
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
                await InitializeUnNumberViewModel(model);
                return View(model);
            }
        }

        private async Task<WasteCodeViewModel> InitializeWasteCodeViewModel(WasteCodeViewModel model)
        {
            var codes = await GetWasteCodes();
            model.EwcCodes = codes.Where(c => c.CodeType == CodeType.Ewc);
            model.WasteCodes = codes.Where(c => c.CodeType == CodeType.Basel || c.CodeType == CodeType.Oecd);

            if (model.SelectedEwcCodes == null)
            {
                model.SelectedEwcCodes = new List<WasteCodeData>();
            }

            return model;
        }

        private async Task<UnNumberViewModel> InitializeUnNumberViewModel(UnNumberViewModel model)
        {
            var codes = await GetWasteCodes();
            model.UnCodes = codes.Where(c => c.CodeType == CodeType.UnNumber);
            if (model.SelectedUnCodes == null)
            {
                model.SelectedUnCodes = new List<WasteCodeData>();
            }
            if (model.CustomCodes == null)
            {
                model.CustomCodes = new List<string>();
            }
            return model;
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