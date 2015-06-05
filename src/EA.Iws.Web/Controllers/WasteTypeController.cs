namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Prsd.Core.Extensions;
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

            //TODO: Redirect to Process of Generation
            return View(model);
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
    }
}