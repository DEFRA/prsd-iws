namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using Core.Notification.Audit;
    using Core.WasteType;
    using Infrastructure;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.WasteType;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.ChemicalComposition;
    using Web.ViewModels.Shared;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class ChemicalCompositionController : Controller
    {
        private const int NumberOfOtherCodesFields = 8;
        private const string NotApplicable = "NA";
        private readonly IMediator mediator;
        private readonly IAuditService auditService;

        private readonly IMapWithParameter<WasteTypeData,
            ICollection<WoodInformationData>,
            ChemicalCompositionViewModel> chemicalCompositionInformationMap;

        public ChemicalCompositionController(IMediator mediator,
            IMapWithParameter<WasteTypeData,
            ICollection<WoodInformationData>,
            ChemicalCompositionViewModel> chemicalCompositionInformationMap,
            IAuditService auditService)
        {
            this.mediator = mediator;
            this.chemicalCompositionInformationMap = chemicalCompositionInformationMap;
            this.auditService = auditService;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, bool? backToOverview = null)
        {
            var model = new ChemicalCompositionTypeViewModel
            {
                NotificationId = id,
                ChemicalCompositionType = RadioButtonStringCollectionViewModel.CreateFromEnum<ChemicalComposition>()
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
        public ActionResult Index(ChemicalCompositionTypeViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            switch (model.ChemicalCompositionType.SelectedValue)
            {
                case "Solid recovered fuel (SRF)":
                    return RedirectToAction("Parameters", new { id = model.NotificationId, chemicalCompositionType = ChemicalComposition.SRF, backToOverview });
                case "Refuse derived fuel (RDF)":
                    return RedirectToAction("Parameters", new { id = model.NotificationId, chemicalCompositionType = ChemicalComposition.RDF, backToOverview });
                case "Wood":
                    return RedirectToAction("Parameters", new { id = model.NotificationId, chemicalCompositionType = ChemicalComposition.Wood, backToOverview });
                default:
                    return RedirectToAction("OtherWaste", new { id = model.NotificationId, chemicalCompositionType = ChemicalComposition.Other, backToOverview });
            }
        }

        [HttpGet]
        public async Task<ActionResult> OtherWaste(Guid id, ChemicalComposition chemicalCompositionType, bool? backToOverview = null)
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

            var existingWasteTypeData = await mediator.SendAsync(new GetWasteType(model.NotificationId));

            await mediator.SendAsync(new CreateWasteType
            {
                NotificationId = model.NotificationId,
                WasteCompositionName = model.Description,
                ChemicalCompositionType = ChemicalComposition.Other
            });

            await this.auditService.AddAuditEntry(this.mediator,
                   model.NotificationId,
                   User.GetUserId(),
                   existingWasteTypeData == null ? NotificationAuditType.Create : NotificationAuditType.Update,
                   NotificationAuditScreenType.ChemicalComposition);

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

            var existingWasteTypeData = await mediator.SendAsync(new GetWasteType(model.NotificationId));

            await mediator.SendAsync(new SetOtherWasteAdditionalInformation(model.NotificationId, model.Description, model.HasAttachement));

            await this.auditService.AddAuditEntry(this.mediator,
                   model.NotificationId,
                   User.GetUserId(),
                   existingWasteTypeData == null ? NotificationAuditType.Create : NotificationAuditType.Update,
                   NotificationAuditScreenType.ChemicalComposition);

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
        public async Task<ActionResult> Parameters(Guid id, ChemicalComposition chemicalCompositionType, bool? backToOverview = null)
        {
            var model = GetBlankCompositionViewModel(id, chemicalCompositionType);
            await GetExistingParameters(id, chemicalCompositionType, model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Parameters(ChemicalCompositionViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingWasteTypeData = await mediator.SendAsync(new GetWasteType(model.NotificationId));

            var filteredWasteCompositions = RemoveNotApplicableValues(model.WasteComposition);

            var createNewWasteType = new CreateWasteType
            {
                NotificationId = model.NotificationId,
                ChemicalCompositionType = model.ChemicalCompositionType,
                WasteCompositions = filteredWasteCompositions
            };

            await mediator.SendAsync(createNewWasteType);
            await mediator.SendAsync(new SetEnergy(model.Energy, model.NotificationId));

            await this.auditService.AddAuditEntry(this.mediator,
                   model.NotificationId,
                   User.GetUserId(),
                   existingWasteTypeData == null ? NotificationAuditType.Create : NotificationAuditType.Update,
                   NotificationAuditScreenType.ChemicalComposition);

            if (model.ChemicalCompositionType == ChemicalComposition.Wood)
            {
                await mediator.SendAsync(new SetWoodTypeDescription(model.Description, model.NotificationId));
            }

            return RedirectToAction("Constituents", new { id = model.NotificationId, chemicalCompositionType = model.ChemicalCompositionType, backToOverview });
        }

        [HttpGet]
        public async Task<ActionResult> Constituents(Guid id, ChemicalComposition chemicalCompositionType, bool? backToOverview = null)
        {
            var model = GetBlankCompositionContinuedViewModel(id, chemicalCompositionType);
            if (chemicalCompositionType == ChemicalComposition.Wood)
            {
                model.WasteComposition = model.WasteComposition.Where(x => x.ChemicalCompositionCategory != ChemicalCompositionCategory.Food).ToList();
            }

            await GetExistingConstituents(id, chemicalCompositionType, model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Constituents(ChemicalCompositionContinuedViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingWasteTypeData = await mediator.SendAsync(new GetWasteType(model.NotificationId));

            //Join optional and mandatory collections
            model.WasteComposition.AddRange(model.OtherCodes);
            
            var blanksRemoved = model.WasteComposition.Where(c => !string.IsNullOrEmpty(c.Constituent));
            
            var filteredWasteCompositions = RemoveNotApplicableValues(blanksRemoved);

            await mediator.SendAsync(new UpdateWasteType(model.NotificationId, model.ChemicalCompositionType, model.FurtherInformation, filteredWasteCompositions));
            await mediator.SendAsync(new SetOptionalInformation(model.FurtherInformation, model.HasAnnex, model.NotificationId));

            await this.auditService.AddAuditEntry(this.mediator,
                   model.NotificationId,
                   User.GetUserId(),
                   existingWasteTypeData == null ? NotificationAuditType.Create : NotificationAuditType.Update,
                   NotificationAuditScreenType.ChemicalComposition);

            if (backToOverview.GetValueOrDefault())
            {
                return RedirectToAction("Index", "Home", new { id = model.NotificationId });
            }

            return RedirectToAction("Index", "WasteGenerationProcess", new { id = model.NotificationId });
        }
        
        private List<WasteTypeCompositionData> GetChemicalCompositionContinuedCategories()
        {
            return Enum.GetValues(typeof(ChemicalCompositionCategory)).Cast<int>()
                .Select(c => new WasteTypeCompositionData { ChemicalCompositionCategory = (ChemicalCompositionCategory)c })
                .Where(c => c.ChemicalCompositionCategory != ChemicalCompositionCategory.Other)
                .ToList();
        }

        private List<WoodInformationData> GetChemicalCompositionCategories()
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

        private async Task GetExistingConstituents(Guid id, ChemicalComposition chemicalCompositionType, ChemicalCompositionContinuedViewModel model)
        {
            var wasteTypeData = await mediator.SendAsync(new GetWasteType(id));

            // If the old data does not exist or corresponds to a different waste type data.
            if (!ContainsData(wasteTypeData) || wasteTypeData.ChemicalCompositionType != chemicalCompositionType)
            {
                while (model.OtherCodes.Count < NumberOfOtherCodesFields)
                {
                    model.OtherCodes.Add(new WasteTypeCompositionData());
                }
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
            model.FurtherInformation = wasteTypeData.FurtherInformation;
            model.HasAnnex = wasteTypeData.HasAnnex;

            while (model.OtherCodes.Count < NumberOfOtherCodesFields)
            {
                model.OtherCodes.Add(new WasteTypeCompositionData());
            }
        }
        private async Task GetExistingParameters(Guid id, ChemicalComposition chemicalCompositionType, ChemicalCompositionViewModel model)
        {
            var wasteTypeData = await mediator.SendAsync(new GetWasteType(id));

            // If the old data does not exist or corresponds to a different waste type data.
            if (!ContainsCompositionData(wasteTypeData) || wasteTypeData.ChemicalCompositionType != chemicalCompositionType)
            {
                return;
            }

            var compositions = wasteTypeData.WasteAdditionalInformation.Select(c => new WoodInformationData
            {
                WasteInformationType = c.WasteInformationType,
                Constituent = c.Constituent,
                MinConcentration = c.MinConcentration.ToString(),
                MaxConcentration = c.MaxConcentration.ToString()
            }).ToList();

            // Where the waste concentration is not applicable it is not stored.
            var notApplicableCompositions =
                model.WasteComposition.Where(
                    wc =>
                        compositions.All(c => c.WasteInformationType != wc.WasteInformationType)).Select(wc => new WoodInformationData
                        {
                            WasteInformationType = wc.WasteInformationType,
                            Constituent = wc.Constituent,
                            MinConcentration = NotApplicable,
                            MaxConcentration = NotApplicable
                        });

            compositions.AddRange(notApplicableCompositions);

            model.WasteComposition = compositions;

            model.Energy = wasteTypeData.EnergyInformation;
            
            if (chemicalCompositionType == ChemicalComposition.Wood)
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

        private bool ContainsCompositionData(WasteTypeData wasteTypeData)
        {
            return wasteTypeData != null
                   && wasteTypeData.WasteAdditionalInformation != null
                   && wasteTypeData.WasteAdditionalInformation.Count > 0;
        }

        private ChemicalCompositionContinuedViewModel GetBlankCompositionContinuedViewModel(Guid id, ChemicalComposition chemicalCompositionType)
        {
            return new ChemicalCompositionContinuedViewModel
            {
                NotificationId = id,
                WasteComposition = GetChemicalCompositionContinuedCategories(),
                OtherCodes = new List<WasteTypeCompositionData> { new WasteTypeCompositionData() },
                ChemicalCompositionType = chemicalCompositionType
            };
        }

        private ChemicalCompositionViewModel GetBlankCompositionViewModel(Guid id, ChemicalComposition chemicalCompositionType)
        {
            return new ChemicalCompositionViewModel
            {
                NotificationId = id,
                WasteComposition = GetChemicalCompositionCategories(),
                ChemicalCompositionType = chemicalCompositionType
            };
        }

        private bool AllOtherCodesFieldsContainData(ChemicalCompositionContinuedViewModel model)
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