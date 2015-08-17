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
                model.Ycodes = await client.SendAsync(User.GetAccessToken(), new GetWasteCodesByType(CodeType.Y));
                model.SelectedYcodesList = new List<WasteCodeData>(await client.SendAsync(User.GetAccessToken(), new GetWasteCodesForNotification(model.NotificationId, CodeType.Y)));
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

                if (model.SelectedYcodesList.Any(c => c.Id.ToString() == remove))
                {
                    model.SelectedYcodesList.RemoveAll(c => c.Id.ToString() == remove);
                }

                return View(model);
            }

            if (command.Equals("addYcode"))
            {
                await InitializeYcodeHcodeAndUnClassViewModel(model);
                WasteCodeData codeToAdd;

                try
                {
                    codeToAdd = model.Ycodes.Single(c => c.Id.ToString() == model.SelectedYcode);
                }
                catch (Exception)
                {
                    return View(model);
                }

                if (codeToAdd.Code.Equals("Not applicable"))
                {
                    model.SelectedYcodesList.Clear();
                    model.SelectedYcodesList.Add(codeToAdd);
                    return View(model);
                }

                if (model.SelectedYcodesList == null)
                {
                    model.SelectedYcodesList = new List<WasteCodeData>();
                }

                if (model.SelectedYcodesList.All(c => c.Id != codeToAdd.Id))
                {
                    if (model.SelectedYcodesList.Any(c => c.Code == "Not applicable"))
                    {
                        model.SelectedYcodesList.RemoveAll(c => c.Code.Equals("Not applicable"));
                    }

                    model.SelectedYcodesList.Add(codeToAdd);
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
                            if (model.SelectedYcodesList.All(x => x.Id != selectedYWasteCode.Id))
                            {
                                model.SelectedYcodesList.Add(selectedYWasteCode);
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
                model.Ycodes = await client.SendAsync(User.GetAccessToken(), new GetWasteCodesByType(CodeType.Y));
                return model;
            }
        }
    }
}