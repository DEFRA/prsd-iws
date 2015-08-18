namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Core.WasteCodes;
    using Infrastructure;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.WasteCodes;
    using ViewModels.WasteCodes;

    [Authorize]
    public class YCodesController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public YCodesController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> EnterYCodes(Guid id)
        {
            var model = new YCodesViewModel
            {
                NotificationId = id
            };

            using (var client = apiClient())
            {
                model.EnterWasteCodesViewModel.Codes = await client.SendAsync(User.GetAccessToken(), new GetWasteCodesByType(CodeType.Y));
                model.EnterWasteCodesViewModel.SelectedCodesList = new List<WasteCodeData>(await client.SendAsync(User.GetAccessToken(), new GetWasteCodesForNotification(model.NotificationId, CodeType.Y)));
                model.EnterWasteCodesViewModel.NotificationId = id;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnterYCodes(YCodesViewModel model, string command, string remove)
        {
            if (remove != null)
            {
                await InitializeYcodeHcodeAndUnClassViewModel(model);
                ModelState.Clear();

                if (model.EnterWasteCodesViewModel.SelectedCodesList.Any(c => c.Id.ToString() == remove))
                {
                    model.EnterWasteCodesViewModel.SelectedCodesList.RemoveAll(c => c.Id.ToString() == remove);
                }

                return View(model);
            }

            if (command.Equals("addYcode"))
            {
                await InitializeYcodeHcodeAndUnClassViewModel(model);
                WasteCodeData codeToAdd;

                try
                {
                    codeToAdd = model.EnterWasteCodesViewModel.Codes.Single(c => c.Id.ToString() == model.EnterWasteCodesViewModel.SelectedCode);
                }
                catch (Exception)
                {
                    return View(model);
                }

                if (codeToAdd.Code.Equals("Not applicable"))
                {
                    model.EnterWasteCodesViewModel.SelectedCodesList.Clear();
                    model.EnterWasteCodesViewModel.SelectedCodesList.Add(codeToAdd);
                    return View(model);
                }

                if (model.EnterWasteCodesViewModel.SelectedCodesList == null)
                {
                    model.EnterWasteCodesViewModel.SelectedCodesList = new List<WasteCodeData>();
                }

                if (model.EnterWasteCodesViewModel.SelectedCodesList.All(c => c.Id != codeToAdd.Id))
                {
                    if (model.EnterWasteCodesViewModel.SelectedCodesList.Any(c => c.Code == "Not applicable"))
                    {
                        model.EnterWasteCodesViewModel.SelectedCodesList.RemoveAll(c => c.Code.Equals("Not applicable"));
                    }

                    model.EnterWasteCodesViewModel.SelectedCodesList.Add(codeToAdd);
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
                    if (model.EnterWasteCodesViewModel.SelectedCode != null)
                    {
                        var selectedYWasteCode = new WasteCodeData { Id = new Guid(model.EnterWasteCodesViewModel.SelectedCode) };
                        if (model.EnterWasteCodesViewModel.SelectedCodesList == null)
                        {
                            model.EnterWasteCodesViewModel.SelectedCodesList = new List<WasteCodeData>();
                            model.EnterWasteCodesViewModel.SelectedCodesList.Add(selectedYWasteCode);
                        }
                        else
                        {
                            if (model.EnterWasteCodesViewModel.SelectedCodesList.All(x => x.Id != selectedYWasteCode.Id))
                            {
                                model.EnterWasteCodesViewModel.SelectedCodesList.Add(selectedYWasteCode);
                            }
                        }
                    }

                    try
                    {
                        //await client.SendAsync(User.GetAccessToken(), new SetYHUnWasteCodes(model.NotificationId, model.SelectedYcodesList.Select(p => p.Id)));
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

        private async Task<YCodesViewModel> InitializeYcodeHcodeAndUnClassViewModel(YCodesViewModel model)
        {
            using (var client = apiClient())
            {
                model.EnterWasteCodesViewModel.Codes = await client.SendAsync(User.GetAccessToken(), new GetWasteCodesByType(CodeType.Y));
                return model;
            }
        }
    }
}