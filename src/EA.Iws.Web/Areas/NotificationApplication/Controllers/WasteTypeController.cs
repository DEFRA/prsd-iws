namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Core.WasteType;
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
                ChemicalCompositionType = RadioButtonStringCollectionViewModel.CreateFromEnum<ChemicalCompositionType>()
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChemicalComposition(ChemicalCompositionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            switch (model.ChemicalCompositionType.SelectedValue)
            {
                case "Solid recovered fuel (SRF)":
                    return RedirectToAction("RdfSrfType", new { id = model.NotificationId, chemicalCompositionType = ChemicalCompositionType.SRF });
                case "Refuse derived fuel (RDF)":
                    return RedirectToAction("RdfSrfType", new { id = model.NotificationId, chemicalCompositionType = ChemicalCompositionType.RDF });
                case "Wood":
                    return RedirectToAction("WoodType", new { id = model.NotificationId, chemicalCompositionType = ChemicalCompositionType.Wood });
                case "Other":
                    return RedirectToAction("OtherWaste", new { id = model.NotificationId, chemicalCompositionType = ChemicalCompositionType.Other });
            }
            return View();
        }

        [HttpGet]
        public ActionResult OtherWaste(Guid id, ChemicalCompositionType chemicalCompositionType)
        {
            var model = new OtherWasteViewModel()
            {
                NotificationId = id
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> OtherWaste(OtherWasteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = apiClient())
            {
                await client.SendAsync(User.GetAccessToken(), new CreateWasteType { NotificationId = model.NotificationId, WasteCompositionName = model.Description, ChemicalCompositionType = ChemicalCompositionType.Other });
            }

            return RedirectToAction("OtherWasteAdditionalInformation", new { id = model.NotificationId });
        }

        [HttpGet]
        public ActionResult OtherWasteAdditionalInformation(Guid id)
        {
            var additionalInformationModel = new OtherWasteAdditionalInformationViewModel()
            {
                NotificationId = id
            };

            return View(additionalInformationModel);
        }

        [HttpPost]
        public async Task<ActionResult> OtherWasteAdditionalInformation(OtherWasteAdditionalInformationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = apiClient())
            {
                await client.SendAsync(User.GetAccessToken(), new SetOtherWasteAdditionalInformation(model.NotificationId, model.Description, model.HasAttachement));
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult WoodType(Guid id, ChemicalCompositionType chemicalCompositionType)
        {
            var model = new ChemicalCompositionConcentrationLevelsViewModel()
            {
                NotificationId = id,
                WasteComposition = GetChemicalCompositionCategorys(),
                OtherCodes = new List<WasteTypeCompositionData> { new WasteTypeCompositionData() },
                ChemicalCompositionType = chemicalCompositionType
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> WoodType(ChemicalCompositionConcentrationLevelsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Command.Equals("add"))
            {
                if (model.OtherCodes == null)
                {
                    model.OtherCodes = new List<WasteTypeCompositionData>();
                }
                model.OtherCodes.Add(new WasteTypeCompositionData());
                return View(model);
            }

            //Join optional and mandatory collections
            model.WasteComposition.AddRange(model.OtherCodes);

            //remove null values
            var blanksRemoved = model.WasteComposition.Where(c => !string.IsNullOrEmpty(c.Constituent));

            //Remove NA values
            var filteredWasteCompositions = blanksRemoved.Where(
                    c => !(c.MinConcentration.ToUpper().Equals("NA") || c.MaxConcentration.ToUpper().Equals("NA") || c.Constituent.ToUpper().Equals("NA"))).ToList();

            var createNewWasteType = new CreateWasteType { NotificationId = model.NotificationId, ChemicalCompositionType = model.ChemicalCompositionType, ChemicalCompositionDescription = model.Description, WasteCompositions = filteredWasteCompositions };

            using (var client = apiClient())
            {
                await client.SendAsync(User.GetAccessToken(), createNewWasteType);
                await client.SendAsync(User.GetAccessToken(), new SetWoodTypeDescription(model.Description, model.NotificationId));
            }

            return RedirectToAction("WoodAdditionalInformation", new { id = model.NotificationId, chemicalCompositionType = model.ChemicalCompositionType });
        }

        [HttpGet]
        public ActionResult RdfSrfType(Guid id, ChemicalCompositionType chemicalCompositionType)
        {
            var model = new ChemicalCompositionConcentrationLevelsViewModel()
            {
                NotificationId = id,
                WasteComposition = GetChemicalCompositionCategorys(),
                OtherCodes = new List<WasteTypeCompositionData> { new WasteTypeCompositionData() },
                ChemicalCompositionType = chemicalCompositionType
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RdfSrfType(ChemicalCompositionConcentrationLevelsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Command.Equals("add"))
            {
                if (model.OtherCodes == null)
                {
                    model.OtherCodes = new List<WasteTypeCompositionData>();
                }
                model.OtherCodes.Add(new WasteTypeCompositionData());
                return View(model);
            }

            //Join optional and mandatory collections
            model.WasteComposition.AddRange(model.OtherCodes);

            //remove null values
            var blanksRemoved = model.WasteComposition.Where(c => !string.IsNullOrEmpty(c.Constituent));

            //Remove NA values
            var filteredWasteCompositions = blanksRemoved.Where(
                    c => !(c.MinConcentration.ToUpper().Equals("NA") || c.MaxConcentration.ToUpper().Equals("NA") || c.Constituent.ToUpper().Equals("NA"))).ToList();

            var createNewWasteType = new CreateWasteType { NotificationId = model.NotificationId, ChemicalCompositionType = model.ChemicalCompositionType, ChemicalCompositionDescription = model.Description, WasteCompositions = filteredWasteCompositions };

            using (var client = apiClient())
            {
                await
                    client.SendAsync(User.GetAccessToken(), createNewWasteType);
            }
            return RedirectToAction("RdfAdditionalInformation", new { id = model.NotificationId, chemicalCompositionType = model.ChemicalCompositionType });
        }

        [HttpGet]
        public ActionResult WoodAdditionalInformation(Guid id, ChemicalCompositionType chemicalCompositionType)
        {
            var categories = Enum.GetValues(typeof(WasteInformationType)).Cast<int>()
                            .Select(c => new WoodInformationData { WasteInformationType = (WasteInformationType)c })
                            .Where(c => c.WasteInformationType != WasteInformationType.Energy)
                            .ToList();

            var model = new ChemicalCompositionInformationViewModel()
            {
                NotificationId = id,
                WasteComposition = categories,
                ChemicalCompositionType = chemicalCompositionType
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> WoodAdditionalInformation(ChemicalCompositionInformationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //Remove NA values
            var filteredWasteCompositions = model.WasteComposition.Where(
                    c => !(c.MinConcentration.ToUpper().Equals("NA") || c.MaxConcentration.ToUpper().Equals("NA") || c.Constituent.ToUpper().Equals("NA"))).ToList();

            using (var client = apiClient())
            {
                await client.SendAsync(User.GetAccessToken(), new UpdateWasteType(model.NotificationId, model.ChemicalCompositionType, model.FurtherInformation, model.Energy, filteredWasteCompositions));
                await client.SendAsync(User.GetAccessToken(), new SetEnergyAndOptionalInformation(model.Energy, model.FurtherInformation, model.NotificationId));
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult RdfAdditionalInformation(Guid id)
        {
            var categories = GetWasteInformationType();

            var model = new ChemicalCompositionInformationViewModel()
            {
                NotificationId = id,
                WasteComposition = categories
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RdfAdditionalInformation(ChemicalCompositionInformationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //Remove NA values
            var filteredWasteCompositions = model.WasteComposition.Where(
                    c => !(c.MinConcentration.ToUpper().Equals("NA") || c.MaxConcentration.ToUpper().Equals("NA") || c.Constituent.ToUpper().Equals("NA"))).ToList();

            using (var client = apiClient())
            {
                await client.SendAsync(User.GetAccessToken(), new UpdateWasteType(model.NotificationId, model.ChemicalCompositionType, model.FurtherInformation, model.Energy, filteredWasteCompositions));
                await client.SendAsync(User.GetAccessToken(), new SetEnergyAndOptionalInformation(model.Energy, model.FurtherInformation, model.NotificationId));
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> WasteGenerationProcess(Guid id)
        {
            using (var client = apiClient())
            {
                var wasteGenerationProcessData =
                    await client.SendAsync(User.GetAccessToken(), new GetWasteGenerationProcess(id));

                var model = new WasteGenerationProcessViewModel(wasteGenerationProcessData);
                return View(model);
            }
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
                    await client.SendAsync(User.GetAccessToken(), model.ToRequest());
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
        public async Task<ActionResult> PhysicalCharacteristics(Guid id)
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
        public async Task<ActionResult> WasteCode(WasteCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await InitializeWasteCodeViewModel(model);
                return View(model);
            }

            if (model.Command.Equals("add"))
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
                    await client.SendAsync(User.GetAccessToken(), new SetWasteCodes(model.SelectedEwcCodes, model.NotificationId));
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
        public async Task<ActionResult> OtherWasteCodes(OtherWasteCodesViewModel model)
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
                    return RedirectToAction("AddYcodeHcodeAndUnClass", new { id = model.NotificationId });
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
        public async Task<ActionResult> UnNumber(UnNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await InitializeUnNumberViewModel(model);
                return View(model);
            }

            if (model.Command.Equals("add"))
            {
                await InitializeUnNumberViewModel(model);
                var codeToAdd = model.UnCodes.Single(c => c.Id.ToString() == model.SelectedUnCode);
                if (model.SelectedUnCodes.All(c => c.Id != codeToAdd.Id))
                {
                    model.SelectedUnCodes.Add(codeToAdd);
                }
                return View(model);
            }

            if (model.Command.Equals("addCustomCode"))
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
                    await client.SendAsync(User.GetAccessToken(), new SetWasteCodes(model.SelectedUnCodes, model.NotificationId));
                    return RedirectToAction("RecoveryPercentage", "RecoveryInfo");
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

        [HttpGet]
        public async Task<ActionResult> AddYcodeHcodeAndUnClass(Guid id)
        {
            var model = new YcodeHcodeAndUnClassViewModel();
            await InitializeYcodeHcodeAndUnClassViewModel(model);
            model.NotificationId = id;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddYcodeHcodeAndUnClass(YcodeHcodeAndUnClassViewModel model, string command)
        {
            //Y codes - non js
            if (command.Equals("addYcode"))
            {
                await InitializeYcodeHcodeAndUnClassViewModel(model);
                var codeToAdd = model.Ycodes.Single(c => c.Id.ToString() == model.SelectedYcode);
                if (model.SelectedYcodesList.All(c => c.Id != codeToAdd.Id))
                {
                    model.SelectedYcodesList.Add(codeToAdd);
                }
                return View(model);
            }

            //H codes - non js
            if (command.Equals("addHcode"))
            {
                await InitializeYcodeHcodeAndUnClassViewModel(model);
                var codeToAdd = model.Hcodes.Single(c => c.Id.ToString() == model.SelectedHcode);
                if (model.SelectedHcodesList.All(c => c.Id != codeToAdd.Id))
                {
                    model.SelectedHcodesList.Add(codeToAdd);
                }
                return View(model);
            }

            //UN classes - non js
            if (command.Equals("addUnClass"))
            {
                await InitializeYcodeHcodeAndUnClassViewModel(model);
                var codeToAdd = model.UnClasses.Single(c => c.Id.ToString() == model.SelectedUnClass);
                if (model.SelectedUnClassesList.All(c => c.Id != codeToAdd.Id))
                {
                    model.SelectedUnClassesList.Add(codeToAdd);
                }
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                await InitializeYcodeHcodeAndUnClassViewModel(model);
                return View(model);
            }

            if (command == "continue")
            {
                using (var client = apiClient())
                {
                    if (model.SelectedYcode != null)
                    {
                        var selectedYWasteCode = new WasteCodeData { Id = new Guid(model.SelectedYcode) };
                        if (model.SelectedYcodesList == null)
                        {
                            model.SelectedYcodesList = new List<WasteCodeData>();
                            model.SelectedYcodesList.Add(selectedYWasteCode);
                        }
                        else
                        {
                            if (!model.SelectedYcodesList.Any(x => x.Id == selectedYWasteCode.Id))
                            {
                                model.SelectedYcodesList.Add(selectedYWasteCode);
                            }
                        }
                    }

                    if (model.SelectedHcode != null)
                    {
                        var selectedHWasteCode = new WasteCodeData { Id = new Guid(model.SelectedHcode) };
                        if (model.SelectedHcodesList == null)
                        {
                            model.SelectedHcodesList = new List<WasteCodeData>();
                            model.SelectedHcodesList.Add(selectedHWasteCode);
                        }
                        else
                        {
                            if (!model.SelectedHcodesList.Any(x => x.Id == selectedHWasteCode.Id))
                            {
                                model.SelectedHcodesList.Add(selectedHWasteCode);
                            }
                        }
                    }

                    if (model.SelectedUnClass != null)
                    {
                        var selectedUnWasteCode = new WasteCodeData { Id = new Guid(model.SelectedUnClass) };
                        if (model.SelectedYcodesList == null)
                        {
                            model.SelectedUnClassesList = new List<WasteCodeData>();
                            model.SelectedUnClassesList.Add(selectedUnWasteCode);
                        }
                        else
                        {
                            if (!model.SelectedUnClassesList.Any(x => x.Id == selectedUnWasteCode.Id))
                            {
                                model.SelectedUnClassesList.Add(selectedUnWasteCode);
                            }
                        }
                    }

                try
                    {
                        await client.SendAsync(User.GetAccessToken(), new SetWasteCodes(model.SelectedYcodesList, model.NotificationId));
                        await client.SendAsync(User.GetAccessToken(), new SetWasteCodes(model.SelectedHcodesList, model.NotificationId));
                        await client.SendAsync(User.GetAccessToken(), new SetWasteCodes(model.SelectedUnClassesList, model.NotificationId));
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
                }
            }

            await InitializeYcodeHcodeAndUnClassViewModel(model);
            return View(model);
        }

        private async Task<YcodeHcodeAndUnClassViewModel> InitializeYcodeHcodeAndUnClassViewModel(YcodeHcodeAndUnClassViewModel model)
        {
            var wasteCodes = await GetWasteCodes();
            model.Ycodes = wasteCodes.Where(c => c.CodeType == CodeType.Y);
            model.Hcodes = wasteCodes.Where(c => c.CodeType == CodeType.H);
            model.UnClasses = wasteCodes.Where(c => c.CodeType == CodeType.Un);

            if (model.SelectedYcodesList == null)
            {
                model.SelectedYcodesList = new List<WasteCodeData>();
            }

            if (model.SelectedHcodesList == null)
            {
                model.SelectedHcodesList = new List<WasteCodeData>();
            }

            if (model.SelectedUnClassesList == null)
            {
                model.SelectedUnClassesList = new List<WasteCodeData>();
            }

            return model;
        }

        private List<WasteTypeCompositionData> GetChemicalCompositionCategorys()
        {
            return Enum.GetValues(typeof(ChemicalCompositionCategory)).Cast<int>()
                .Select(c => new WasteTypeCompositionData { ChemicalCompositionCategory = (ChemicalCompositionCategory)c })
                .Where(c => c.ChemicalCompositionCategory != ChemicalCompositionCategory.Other)
                .ToList();
        }

        private List<WoodInformationData> GetWasteInformationType()
        {
            return Enum.GetValues(typeof(WasteInformationType)).Cast<int>()
                .Select(c => new WoodInformationData { WasteInformationType = (WasteInformationType)c })
                .Where(c => c.WasteInformationType != WasteInformationType.Energy)
                .ToList();
        }
    }
}