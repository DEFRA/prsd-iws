﻿namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Notification.Audit;
    using Core.WasteType;
    using EA.Iws.Core.WasteComponentType;
    using EA.Iws.Requests.WasteComponentType;
    using EA.Iws.Web.Areas.NotificationApplication.Views.ChemicalComposition;
    using EA.Prsd.Core.Helpers;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using Requests.WasteType;
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

        public ChemicalCompositionController(IMediator mediator, IAuditService auditService)
        {
            this.mediator = mediator;
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
                model.ChemicalCompositionType.SelectedValue = EnumHelper.GetDisplayName(wasteTypeData.ChemicalCompositionType);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ChemicalCompositionTypeViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                if (ModelState["ChemicalCompositionType.SelectedValue"] != null && ModelState["ChemicalCompositionType.SelectedValue"].Errors.Count == 1)
                {
                    ModelState["ChemicalCompositionType.SelectedValue"].Errors.Clear();
                    ModelState.AddModelError("ChemicalCompositionType.SelectedValue", "Please tell us what waste type applies");
                }
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
                    return RedirectToAction("WasteCategory", "ChemicalComposition", new { notificationId = model.NotificationId });
            }
        }

        [HttpGet]
        public async Task<ActionResult> WasteCategory(Guid notificationId)
        {
            var model = new WasteCategoryViewModel
            {
                NotificationId = notificationId,
                WasteCategoryType = RadioButtonStringCollectionViewModel.CreateFromEnum<WasteCategoryType>()
            };

            var wasteTypeData = await mediator.SendAsync(new GetWasteType(notificationId));
            if (wasteTypeData != null && wasteTypeData.WasteCategoryType != null)
            {
                model.WasteCategoryType.SelectedValue = EnumHelper.GetDisplayName((WasteCategoryType)wasteTypeData.WasteCategoryType);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> WasteCategory(WasteCategoryViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                if (ModelState["WasteCategoryType.SelectedValue"] != null && ModelState["WasteCategoryType.SelectedValue"].Errors.Count == 1)
                {
                    ModelState["WasteCategoryType.SelectedValue"].Errors.Clear();
                    ModelState.AddModelError("WasteCategoryType.SelectedValue", "Select the appropriate waste category");
                }
                return View(model);
            }

            var existingWasteTypeData = await mediator.SendAsync(new GetWasteType(model.NotificationId));

            var position = model.WasteCategoryType.SelectedValue.IndexOf("/");
            var selectedCategoryName = string.Concat(model.WasteCategoryType.SelectedValue.Where(c => !char.IsWhiteSpace(c)));
            if (position >= 0)
            {
                selectedCategoryName = model.WasteCategoryType.SelectedValue.Remove(position, 1);
            }

            await mediator.SendAsync(new CreateWasteType
            {
                NotificationId = model.NotificationId,
                ChemicalCompositionType = ChemicalComposition.Other,
                WasteCategoryType = (WasteCategoryType)Enum.Parse(typeof(WasteCategoryType), selectedCategoryName)
            });

            await auditService.AddAuditEntry(mediator,
                   model.NotificationId,
                   User.GetUserId(),
                   existingWasteTypeData == null ? NotificationAuditType.Added : NotificationAuditType.Updated,
                   NotificationAuditScreenType.ChemicalComposition);

            return RedirectToAction("WasteComponent", "ChemicalComposition", new { notificationId = model.NotificationId, backToOverview });
        }

        [HttpGet]
        public async Task<ActionResult> WasteComponent(Guid notificationId, bool? backToOverview = null)
        {
            var wasteComponentTypes = CheckBoxCollectionViewModel.CreateFromEnum<WasteComponentType>();
            wasteComponentTypes.ShowEnumValue = true;
            wasteComponentTypes.PossibleValues = wasteComponentTypes.PossibleValues.ToList();

            var model = new WasteComponentViewModel
            {
                NotificationId = notificationId,
                WasteComponentTypes = wasteComponentTypes
            };

            var wasteComponentData = await mediator.SendAsync(new GetWasteComponentInfoForNotification(notificationId));
            if (wasteComponentData != null)
            {
                model.WasteComponentTypes.SetSelectedValues(wasteComponentData.WasteComponentTypes);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> WasteComponent(WasteComponentViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var selectedWasteComponentTypes = model.WasteComponentTypes.PossibleValues.Where(p => p.Selected).Select(p => (WasteComponentType)(Convert.ToInt32(p.Value))).ToList();
            var existingWasteComponentData = await mediator.SendAsync(new GetWasteComponentInfoForNotification(model.NotificationId));
            await mediator.SendAsync(new SetWasteComponentInfoForNotification(selectedWasteComponentTypes, model.NotificationId));

            await auditService.AddAuditEntry(mediator,
                model.NotificationId,
                User.GetUserId(),
                existingWasteComponentData.WasteComponentTypes.Count == 0 ? NotificationAuditType.Added : NotificationAuditType.Updated,
                NotificationAuditScreenType.ChemicalComposition);

            return RedirectToAction("OtherWaste", "ChemicalComposition", new { notificationId = model.NotificationId, backToOverview });
        }

        [HttpGet]
        public async Task<ActionResult> OtherWaste(Guid notificationId, bool? backToOverview = null)
        {
            var model = new OtherWasteViewModel
            {
                NotificationId = notificationId
            };

            var wasteTypeData = await mediator.SendAsync(new GetWasteType(notificationId));

            if (wasteTypeData != null && wasteTypeData.ChemicalCompositionName != null)
            {
                model.Description = wasteTypeData.ChemicalCompositionName;
            }

            if (wasteTypeData != null && wasteTypeData.WasteCategoryType != null)
            {
                model.WasteCategoryType = wasteTypeData.WasteCategoryType.Value;
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
                ChemicalCompositionType = ChemicalComposition.Other,
                WasteCategoryType = model.WasteCategoryType
            });

            await this.auditService.AddAuditEntry(this.mediator,
                   model.NotificationId,
                   User.GetUserId(),
                   NotificationAuditType.Updated,
                   NotificationAuditScreenType.ChemicalComposition);

            return RedirectToAction("OtherWasteAdditionalInformation", "ChemicalComposition", new { id = model.NotificationId, backToOverview });
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
                   existingWasteTypeData == null ? NotificationAuditType.Added : NotificationAuditType.Updated,
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

            bool dataHasChanged = false;

            if (existingWasteTypeData != null)
            {
                dataHasChanged = CheckForChangesInFirstScreen(existingWasteTypeData, filteredWasteCompositions, model.Energy, model.ChemicalCompositionType);
            }

            if (existingWasteTypeData == null || dataHasChanged)
            {
                await this.auditService.AddAuditEntry(this.mediator,
                       model.NotificationId,
                       User.GetUserId(),
                       dataHasChanged == false ? NotificationAuditType.Added : NotificationAuditType.Updated,
                       NotificationAuditScreenType.ChemicalComposition);
            }

            if (model.ChemicalCompositionType == ChemicalComposition.Wood)
            {
                await mediator.SendAsync(new SetWoodTypeDescription(model.Description, model.NotificationId));
            }

            return RedirectToAction("Constituents", new { id = model.NotificationId, chemicalCompositionType = model.ChemicalCompositionType, backToOverview });
        }

        private bool CheckForChangesInFirstScreen(WasteTypeData existingWasteTypeData, List<WoodInformationData> filteredWasteCompositions, string energy, ChemicalComposition chemicalCompositionType)
        {
            if (existingWasteTypeData.EnergyInformation != energy)
            {
                return true;
            }

            if (existingWasteTypeData.ChemicalCompositionType != chemicalCompositionType)
            {
                return true;
            }

            foreach (var newData in filteredWasteCompositions)
            {
                var existingData = existingWasteTypeData.WasteAdditionalInformation.FirstOrDefault(p => p.WasteInformationType == newData.WasteInformationType);

                if (existingData.Constituent != newData.Constituent || Double.Parse(existingData.MaxConcentration) != Double.Parse(newData.MaxConcentration) || Double.Parse(existingData.MinConcentration) != Double.Parse(newData.MinConcentration))
                {
                    return true;
                }
            }

            return false;
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

            bool dataHasChanged = false;

            string firstScreensLastAuditType = await GetFirstScreensLastAuditType(model.NotificationId);

            if (firstScreensLastAuditType == NotificationAuditType.Added.ToString() && existingWasteTypeData.WasteCompositionData.Count == 0)
            {
                await this.auditService.AddAuditEntry(this.mediator,
                       model.NotificationId,
                       User.GetUserId(),
                       NotificationAuditType.Added,
                       NotificationAuditScreenType.ChemicalCompositionContinued);
            }
            else
            {
                if (existingWasteTypeData.WasteCompositionData.Count != 0)
                {
                    dataHasChanged = CheckForChangesInSecondScreen(existingWasteTypeData, filteredWasteCompositions, model.FurtherInformation, model.HasAnnex, model.ChemicalCompositionType);

                    if (dataHasChanged)
                    {
                        await this.auditService.AddAuditEntry(this.mediator,
                           model.NotificationId,
                           User.GetUserId(),
                           NotificationAuditType.Updated,
                           NotificationAuditScreenType.ChemicalCompositionContinued);
                    }
                }
                else
                {
                    await this.auditService.AddAuditEntry(this.mediator,
                       model.NotificationId,
                       User.GetUserId(),
                       NotificationAuditType.Updated,
                       NotificationAuditScreenType.ChemicalCompositionContinued);
                }
            }

            if (backToOverview.GetValueOrDefault())
            {
                return RedirectToAction("Index", "Home", new { id = model.NotificationId });
            }

            return RedirectToAction("Index", "WasteGenerationProcess", new { id = model.NotificationId });
        }

        private async Task<string> GetFirstScreensLastAuditType(Guid id)
        {
            var response = await mediator.SendAsync(new GetNotificationAuditTable(id, 1, (int)NotificationAuditScreenType.ChemicalComposition, null, null));

            return response.TableData.OrderByDescending(p => p.DateAdded).FirstOrDefault().AuditType;
        }

        private bool CheckForChangesInSecondScreen(WasteTypeData existingWasteTypeData, List<WasteTypeCompositionData> filteredWasteCompositions, string futherInformation, bool hasAnnex, ChemicalComposition chemicalCompositionType)
        {
            if (chemicalCompositionType != existingWasteTypeData.ChemicalCompositionType)
            {
                return true;
            }

            if (futherInformation != existingWasteTypeData.FurtherInformation)
            {
                return true;
            }

            if (hasAnnex != existingWasteTypeData.HasAnnex)
            {
                return true;
            }

            if (filteredWasteCompositions.Count != existingWasteTypeData.WasteCompositionData.Count)
            {
                return true;
            }

            foreach (var newData in filteredWasteCompositions)
            {
                var existingData = existingWasteTypeData.WasteCompositionData.FirstOrDefault(p => p.ChemicalCompositionCategory == newData.ChemicalCompositionCategory);

                if (existingData == null)
                {
                    return true;
                }

                if (existingData.Constituent != newData.Constituent || existingData.MaxConcentration != Decimal.Parse(newData.MaxConcentration) || existingData.MinConcentration != Decimal.Parse(newData.MinConcentration) || existingData.Constituent != newData.Constituent)
                {
                    return true;
                }
            }

            return false;
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
    }
}