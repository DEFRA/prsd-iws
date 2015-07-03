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
        public async Task<ActionResult> ChemicalComposition(Guid id)
        {
            var model = new ChemicalCompositionViewModel
            {
                NotificationId = id,
                ChemicalCompositionType = RadioButtonStringCollectionViewModel.CreateFromEnum<ChemicalCompositionType>()
            };

            using (var client = apiClient())
            {
                var wasteTypeData = await client.SendAsync(User.GetAccessToken(), new GetWasteType(id));
                if (wasteTypeData != null)
                {
                    model.ChemicalCompositionType.SelectedValue = Prsd.Core.Helpers.EnumHelper.GetDisplayName(wasteTypeData.ChemicalCompositionType);
                }
            }

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
        public async Task<ActionResult> OtherWaste(Guid id, ChemicalCompositionType chemicalCompositionType)
        {
            var model = new OtherWasteViewModel()
            {
                NotificationId = id
            };

            using (var client = apiClient())
            {
                var wasteTypeData = await client.SendAsync(User.GetAccessToken(), new GetWasteType(id));
                if (wasteTypeData != null && wasteTypeData.ChemicalCompositionName != null)
                {
                    model.Description = wasteTypeData.ChemicalCompositionName;
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public async Task<ActionResult> OtherWasteAdditionalInformation(Guid id)
        {
            var model = new OtherWasteAdditionalInformationViewModel()
            {
                NotificationId = id
            };

            using (var client = apiClient())
            {
                var wasteTypeData = await client.SendAsync(User.GetAccessToken(), new GetWasteType(id));
                if (wasteTypeData != null)
                {
                    model.Description = wasteTypeData.OtherWasteTypeDescription;
                    model.HasAttachement = wasteTypeData.HasAnnex;
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

            return RedirectToAction("Index", "WasteGenerationProcess", new { id = model.NotificationId });
        }

        [HttpGet]
        public async Task<ActionResult> WoodType(Guid id, ChemicalCompositionType chemicalCompositionType)
        {
            var model = new ChemicalCompositionConcentrationLevelsViewModel()
            {
                NotificationId = id,
                WasteComposition = GetChemicalCompositionCategorys(),
                OtherCodes = new List<WasteTypeCompositionData> { new WasteTypeCompositionData() },
                ChemicalCompositionType = chemicalCompositionType
            };

            await GetExistingCompositions(id, chemicalCompositionType, model);

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
        public async Task<ActionResult> RdfSrfType(Guid id, ChemicalCompositionType chemicalCompositionType)
        {
            var model = new ChemicalCompositionConcentrationLevelsViewModel()
            {
                NotificationId = id,
                WasteComposition = GetChemicalCompositionCategorys(),
                OtherCodes = new List<WasteTypeCompositionData> { new WasteTypeCompositionData() },
                ChemicalCompositionType = chemicalCompositionType
            };

            await GetExistingCompositions(id, chemicalCompositionType, model);

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
            var filteredWasteCompositions = blanksRemoved.Where(c => !(c.MinConcentration.ToUpper().Equals("NA") || c.MaxConcentration.ToUpper().Equals("NA") || c.Constituent.ToUpper().Equals("NA"))).ToList();

            var createNewWasteType = new CreateWasteType { NotificationId = model.NotificationId, ChemicalCompositionType = model.ChemicalCompositionType, ChemicalCompositionDescription = model.Description, WasteCompositions = filteredWasteCompositions };

            using (var client = apiClient())
            {
                await client.SendAsync(User.GetAccessToken(), createNewWasteType);
            }
            return RedirectToAction("RdfAdditionalInformation", new { id = model.NotificationId, chemicalCompositionType = model.ChemicalCompositionType });
        }

        [HttpGet]
        public async Task<ActionResult> WoodAdditionalInformation(Guid id, ChemicalCompositionType chemicalCompositionType)
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

            await GetExistingAdditionalInformation(id, model);

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
            return RedirectToAction("Index", "WasteGenerationProcess", new { id = model.NotificationId });
        }

        [HttpGet]
        public async Task<ActionResult> RdfAdditionalInformation(Guid id)
        {
            var categories = GetWasteInformationType();

            var model = new ChemicalCompositionInformationViewModel()
            {
                NotificationId = id,
                WasteComposition = categories
            };

            await GetExistingAdditionalInformation(id, model);

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
            return RedirectToAction("Index", "WasteGenerationProcess", new { id = model.NotificationId });
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

        private async Task GetExistingAdditionalInformation(Guid id, ChemicalCompositionInformationViewModel model)
        {
            using (var client = apiClient())
            {
                var wasteTypeData = await client.SendAsync(User.GetAccessToken(), new GetWasteType(id));
                if (wasteTypeData != null && wasteTypeData.WasteAdditionalInformarion != null && wasteTypeData.WasteAdditionalInformarion.Count > 0)
                {
                    model.WasteComposition = wasteTypeData.WasteAdditionalInformarion;
                    model.Energy = wasteTypeData.EnergyInformation;
                    model.FurtherInformation = wasteTypeData.FurtherInformation;
                }
            }
        }

        private async Task GetExistingCompositions(Guid id, ChemicalCompositionType chemicalCompositionType, ChemicalCompositionConcentrationLevelsViewModel model)
        {
            using (var client = apiClient())
            {
                var wasteTypeData = await client.SendAsync(User.GetAccessToken(), new GetWasteType(id));
                if (wasteTypeData != null && wasteTypeData.WasteCompositionData != null && wasteTypeData.WasteCompositionData.Count > 0 && wasteTypeData.ChemicalCompositionType == chemicalCompositionType)
                {
                    var compositions = wasteTypeData.WasteCompositionData.Select(c => new WasteTypeCompositionData
                    {
                        ChemicalCompositionCategory = c.ChemicalCompositionCategory,
                        Constituent = c.Constituent,
                        MinConcentration = c.MinConcentration.ToString(),
                        MaxConcentration = c.MaxConcentration.ToString()
                    }).ToList();

                    model.WasteComposition = compositions.Where(c => c.ChemicalCompositionCategory != ChemicalCompositionCategory.Other).ToList();
                    model.OtherCodes = compositions.Where(c => c.ChemicalCompositionCategory == ChemicalCompositionCategory.Other).ToList();

                    if (model.OtherCodes.Count == 0)
                    {
                        model.OtherCodes.Add(new WasteTypeCompositionData());
                    }

                    if (chemicalCompositionType == ChemicalCompositionType.Wood)
                    {
                        model.Description = wasteTypeData.WoodTypeDescription;
                    }
                }
            }
        }
    }
}