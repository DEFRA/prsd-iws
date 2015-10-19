namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.WasteType;
    using Infrastructure;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.WasteType;
    using ViewModels.WasteType;
    using Web.ViewModels.Shared;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class WasteTypeController : Controller
    {
        private const string NotApplicable = "NA";
        private readonly IMediator mediator;

        private readonly IMapWithParameter<WasteTypeData,
            ICollection<WoodInformationData>,
            ChemicalCompositionInformationViewModel> chemicalCompositionInformationMap;

        public WasteTypeController(IMediator mediator,
            IMapWithParameter<WasteTypeData,
            ICollection<WoodInformationData>,
            ChemicalCompositionInformationViewModel> chemicalCompositionInformationMap)
        {
            this.mediator = mediator;
            this.chemicalCompositionInformationMap = chemicalCompositionInformationMap;
        }

        [HttpGet]
        public async Task<ActionResult> ChemicalComposition(Guid id, bool? backToOverview = null)
        {
            var model = new ChemicalCompositionViewModel
            {
                NotificationId = id,
                ChemicalCompositionType = RadioButtonStringCollectionViewModel.CreateFromEnum<ChemicalCompositionType>()
            };

            var wasteTypeData = await mediator.SendAsync(new GetWasteType(id));
            if (wasteTypeData != null)
            {
                model.ChemicalCompositionType.SelectedValue = Prsd.Core.Helpers.EnumHelper.GetDisplayName(wasteTypeData.ChemicalCompositionType);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChemicalComposition(ChemicalCompositionViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            switch (model.ChemicalCompositionType.SelectedValue)
            {
                case "Solid recovered fuel (SRF)":
                    return RedirectToAction("RdfSrfType", new { id = model.NotificationId, chemicalCompositionType = ChemicalCompositionType.SRF, backToOverview });
                case "Refuse derived fuel (RDF)":
                    return RedirectToAction("RdfSrfType", new { id = model.NotificationId, chemicalCompositionType = ChemicalCompositionType.RDF, backToOverview });
                case "Wood":
                    return RedirectToAction("WoodType", new { id = model.NotificationId, chemicalCompositionType = ChemicalCompositionType.Wood, backToOverview });
                default:
                    return RedirectToAction("OtherWaste", new { id = model.NotificationId, chemicalCompositionType = ChemicalCompositionType.Other, backToOverview });
            }
        }

        [HttpGet]
        public async Task<ActionResult> OtherWaste(Guid id, ChemicalCompositionType chemicalCompositionType, bool? backToOverview = null)
        {
            var model = new OtherWasteViewModel
            {
                NotificationId = id
            };

            var wasteTypeData = await mediator.SendAsync(new GetWasteType(id));

            if (wasteTypeData != null && wasteTypeData.ChemicalCompositionName != null)
            {
                model.Description = wasteTypeData.ChemicalCompositionName;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> OtherWaste(OtherWasteViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await mediator.SendAsync(new CreateWasteType
            {
                NotificationId = model.NotificationId,
                WasteCompositionName = model.Description,
                ChemicalCompositionType = ChemicalCompositionType.Other
            });

            return RedirectToAction("OtherWasteAdditionalInformation", new { id = model.NotificationId, backToOverview });
        }

        [HttpGet]
        public async Task<ActionResult> OtherWasteAdditionalInformation(Guid id, bool? backToOverview = null)
        {
            var model = new OtherWasteAdditionalInformationViewModel
            {
                NotificationId = id
            };

            var wasteTypeData = await mediator.SendAsync(new GetWasteType(id));
            if (wasteTypeData != null)
            {
                model.Description = wasteTypeData.OtherWasteTypeDescription;
                model.HasAttachement = wasteTypeData.HasAnnex;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> OtherWasteAdditionalInformation(OtherWasteAdditionalInformationViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            await mediator.SendAsync(new SetOtherWasteAdditionalInformation(model.NotificationId, model.Description, model.HasAttachement));

            if (backToOverview.GetValueOrDefault())
            {
                return RedirectToAction("Index", "Home", new { id = model.NotificationId });
            }
            else
            {
                return RedirectToAction("Index", "WasteGenerationProcess", new { id = model.NotificationId });
            }
        }

        [HttpGet]
        public async Task<ActionResult> WoodType(Guid id, ChemicalCompositionType chemicalCompositionType, bool? backToOverview = null)
        {
            var model = GetViewModelForWood(id, chemicalCompositionType);

            await GetExistingCompositions(id, chemicalCompositionType, model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> WoodType(ChemicalCompositionConcentrationLevelsViewModel model, bool? backToOverview = null)
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

                if (AllOtherCodesFieldsContainData(model))
                {
                    model.OtherCodes.Add(new WasteTypeCompositionData());
                }

                return View(model);
            }

            // Join optional and mandatory collections
            model.WasteComposition.AddRange(model.OtherCodes);

            var blanksRemoved = model.WasteComposition.Where(c => !string.IsNullOrEmpty(c.Constituent));

            var filteredWasteCompositions = RemoveNotApplicableValues(blanksRemoved);

            var createNewWasteType = new CreateWasteType
            {
                NotificationId = model.NotificationId,
                ChemicalCompositionType = model.ChemicalCompositionType,
                ChemicalCompositionDescription = model.Description,
                WasteCompositions = filteredWasteCompositions
            };
            
            await mediator.SendAsync(createNewWasteType);
            await mediator.SendAsync(new SetWoodTypeDescription(model.Description, model.NotificationId));

            return RedirectToAction("WoodAdditionalInformation", new { id = model.NotificationId, chemicalCompositionType = model.ChemicalCompositionType, backToOverview });
        }

        [HttpGet]
        public async Task<ActionResult> RdfSrfType(Guid id, ChemicalCompositionType chemicalCompositionType, bool? backToOverview = null)
        {
            var model = GetBlankViewModel(id, chemicalCompositionType);

            await GetExistingCompositions(id, chemicalCompositionType, model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RdfSrfType(ChemicalCompositionConcentrationLevelsViewModel model, bool? backToOverview = null)
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

                if (AllOtherCodesFieldsContainData(model))
                {
                    model.OtherCodes.Add(new WasteTypeCompositionData());
                }

                return View(model);
            }

            //Join optional and mandatory collections
            model.WasteComposition.AddRange(model.OtherCodes);

            //remove null values
            var blanksRemoved = model.WasteComposition.Where(c => !string.IsNullOrEmpty(c.Constituent));

            //Remove NA values
            var filteredWasteCompositions = RemoveNotApplicableValues(blanksRemoved);

            var createNewWasteType = new CreateWasteType
            {
                NotificationId = model.NotificationId,
                ChemicalCompositionType = model.ChemicalCompositionType,
                ChemicalCompositionDescription = model.Description,
                WasteCompositions = filteredWasteCompositions
            };

            await mediator.SendAsync(createNewWasteType);

            return RedirectToAction("RdfAdditionalInformation", new { id = model.NotificationId, chemicalCompositionType = model.ChemicalCompositionType, backToOverview });
        }

        [HttpGet]
        public async Task<ActionResult> WoodAdditionalInformation(Guid id, ChemicalCompositionType chemicalCompositionType, bool? backToOverview = null)
        {
            var result = await mediator.SendAsync(new GetWasteType(id));
            var categories = GetWasteInformationType().Where(c => c.WasteInformationType != WasteInformationType.Energy);

            var model = chemicalCompositionInformationMap.Map(result, categories.ToArray());
            model.NotificationId = id;
            model.ChemicalCompositionType = chemicalCompositionType;
                
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> WoodAdditionalInformation(ChemicalCompositionInformationViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            await
                mediator.SendAsync(new UpdateWasteType(model.NotificationId, model.ChemicalCompositionType,
                        model.FurtherInformation, model.Energy, RemoveNotApplicableValues(model.WasteComposition)));
            await
                mediator.SendAsync(new SetEnergyAndOptionalInformation(model.Energy, model.FurtherInformation, model.HasAnnex,
                        model.NotificationId));
            
            if (backToOverview.GetValueOrDefault())
            {
                return RedirectToAction("Index", "Home", new { id = model.NotificationId });
            }
            else
            {
                return RedirectToAction("Index", "WasteGenerationProcess", new { id = model.NotificationId });
            }
        }

        [HttpGet]
        public async Task<ActionResult> RdfAdditionalInformation(Guid id, bool? backToOverview = null)
        {
            var result = await mediator.SendAsync(new GetWasteType(id));
            var categories = GetWasteInformationType();

            var model = chemicalCompositionInformationMap.Map(result, categories);
            model.NotificationId = id;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RdfAdditionalInformation(ChemicalCompositionInformationViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var filteredWasteCompositions = RemoveNotApplicableValues(model.WasteComposition);

            await mediator.SendAsync(new UpdateWasteType(model.NotificationId, model.ChemicalCompositionType, model.FurtherInformation, model.Energy, filteredWasteCompositions));
            await mediator.SendAsync(new SetEnergyAndOptionalInformation(model.Energy, model.FurtherInformation, model.HasAnnex, model.NotificationId));

            if (backToOverview.GetValueOrDefault())
            {
                return RedirectToAction("Index", "Home", new { id = model.NotificationId });
            }
            else
            {
                return RedirectToAction("Index", "WasteGenerationProcess", new { id = model.NotificationId });
            }
        }

        private List<WasteTypeCompositionData> GetChemicalCompositionCategories()
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

        private List<T> RemoveNotApplicableValues<T>(IEnumerable<T> data) where T : ChemicalCompositionData
        {
            return data.Where(
                c =>
                    !(c.MinConcentration.ToUpper().Equals(NotApplicable) ||
                      c.MaxConcentration.ToUpper().Equals(NotApplicable) ||
                      c.Constituent.ToUpper().Equals(NotApplicable))).ToList();
        }

        private async Task GetExistingCompositions(Guid id, ChemicalCompositionType chemicalCompositionType, ChemicalCompositionConcentrationLevelsViewModel model)
        {
            var wasteTypeData = await mediator.SendAsync(new GetWasteType(id));

            // If the old data does not exist or corresponds to a different waste type data.
            if (!ContainsData(wasteTypeData) || wasteTypeData.ChemicalCompositionType != chemicalCompositionType)
            {
                return;
            }

            var compositions = wasteTypeData.WasteCompositionData.Select(c => new WasteTypeCompositionData
                {
                    ChemicalCompositionCategory = c.ChemicalCompositionCategory,
                    Constituent = c.Constituent,
                    MinConcentration = c.MinConcentration.ToString(),
                    MaxConcentration = c.MaxConcentration.ToString()
                }).ToList();

            // Where the waste concentration is not applicable it is not stored.
            var notApplicableCompositions =
                model.WasteComposition.Where(
                    wc =>
                        compositions.All(c => c.ChemicalCompositionCategory != wc.ChemicalCompositionCategory)).Select(wc => new WasteTypeCompositionData
                        {
                            ChemicalCompositionCategory = wc.ChemicalCompositionCategory,
                            Constituent = wc.Constituent,
                            MinConcentration = NotApplicable,
                            MaxConcentration = NotApplicable
                        });

            compositions.AddRange(notApplicableCompositions);

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

        private bool ContainsData(WasteTypeData wasteTypeData)
        {
            return wasteTypeData != null
                   && wasteTypeData.WasteCompositionData != null
                   && wasteTypeData.WasteCompositionData.Count > 0;
        }

        private ChemicalCompositionConcentrationLevelsViewModel GetBlankViewModel(Guid id, ChemicalCompositionType chemicalCompositionType)
        {
            return new ChemicalCompositionConcentrationLevelsViewModel
            {
                NotificationId = id,
                WasteComposition = GetChemicalCompositionCategories(),
                OtherCodes = new List<WasteTypeCompositionData> { new WasteTypeCompositionData() },
                ChemicalCompositionType = chemicalCompositionType
            };
        }

        private ChemicalCompositionConcentrationLevelsViewModel GetViewModelForWood(Guid id, ChemicalCompositionType chemicalCompositionType)
        {
            var woodCompositions = GetBlankViewModel(id, chemicalCompositionType);
            woodCompositions.WasteComposition = woodCompositions.WasteComposition.Where(x => x.ChemicalCompositionCategory != ChemicalCompositionCategory.Food).ToList();

            return woodCompositions;
        }

        private bool AllOtherCodesFieldsContainData(ChemicalCompositionConcentrationLevelsViewModel model)
        {
            var result = true;

            foreach (var i in model.OtherCodes)
            {
                if (string.IsNullOrWhiteSpace(i.Constituent))
                {
                    result = false;
                }
            }

            return result;
        }
    }
}