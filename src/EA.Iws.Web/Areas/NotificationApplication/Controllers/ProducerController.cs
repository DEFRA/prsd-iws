namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Producers;
    using ViewModels.Producer;
    using Web.ViewModels.Shared;

    [Authorize]
    public class ProducerController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public ProducerController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public ActionResult CopyFromExporter(Guid id)
        {
            var model = new YesNoChoiceViewModel();
            ViewBag.NotificationId = id;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CopyFromExporter(Guid id, YesNoChoiceViewModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.NotificationId = id;
                return View(inputModel);
            }

            if (inputModel.Choices.SelectedValue.Equals("Yes"))
            {
                using (var client = apiClient())
                {
                    await client.SendAsync(User.GetAccessToken(), new CopyProducerFromExporter(id));
                }
            }

            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<ActionResult> Add(Guid id)
        {
            var model = new AddProducerViewModel { NotificationId = id };

            await this.BindCountryList(apiClient);
            model.Address.DefaultCountryId = this.GetDefaultCountryId();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(AddProducerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await this.BindCountryList(apiClient);
                return View(model);
            }

            using (var client = apiClient())
            {
                try
                {
                    var request = model.ToRequest();

                    await client.SendAsync(User.GetAccessToken(), request);

                    return RedirectToAction("List", "Producer",
                        new { id = model.NotificationId });
                }
                catch (ApiBadRequestException ex)
                {
                    this.HandleBadRequest(ex);

                    if (ModelState.IsValid)
                    {
                        throw;
                    }
                }
                await this.BindCountryList(client);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Edit(Guid id, Guid entityId)
        {
            using (var client = apiClient())
            {
                var producer =
                    await client.SendAsync(User.GetAccessToken(), new GetProducerForNotification(id, entityId));

                var model = new EditProducerViewModel(producer);

                await this.BindCountryList(client);
                model.Address.DefaultCountryId = this.GetDefaultCountryId();
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditProducerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await this.BindCountryList(apiClient);
                return View(model);
            }

            using (var client = apiClient())
            {
                try
                {
                    var request = model.ToRequest();

                    await client.SendAsync(User.GetAccessToken(), request);

                    return RedirectToAction("List", "Producer",
                        new { id = model.NotificationId });
                }
                catch (ApiBadRequestException ex)
                {
                    this.HandleBadRequest(ex);

                    if (ModelState.IsValid)
                    {
                        throw;
                    }
                }
                await this.BindCountryList(client);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Remove(Guid id, Guid entityId)
        {
            using (var client = apiClient())
            {
                try
                {
                    var response = await client.SendAsync(User.GetAccessToken(), new GetProducersByNotificationId(id));
                    var producers = response.ToList();
                    var producersCount = producers.Count;
                    var isSiteOfExport = producers.Single(x => x.Id == entityId).IsSiteOfExport;

                    if (isSiteOfExport && producersCount > 1)
                    {
                        TempData["errorRemoveProducer"] =
                            "You have chosen to remove the site of export. You will need to select an alternative site of export before you can remove this producer.";
                        return RedirectToAction("List", "Producer", new { id });
                    }

                    await client.SendAsync(User.GetAccessToken(), new DeleteProducerForNotification(entityId, id));
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
            return RedirectToAction("List", "Producer", new { id });
        }

        [HttpGet]
        public async Task<ActionResult> List(Guid id)
        {
            var model = new MultipleProducersViewModel();
            if (TempData["errorRemoveProducer"] != null)
            {
                ModelState.AddModelError("RemoveProducer", TempData["errorRemoveProducer"].ToString());
            }

            using (var client = apiClient())
            {
                var response =
                    await client.SendAsync(User.GetAccessToken(), new GetProducersByNotificationId(id));

                try
                {
                    model.NotificationId = id;
                    model.ProducerData = response.ToList();
                    model.HasSiteOfExport = model.ProducerData.Exists(p => p.IsSiteOfExport);
                    return View(model);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetSiteOfExport(MultipleProducersViewModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.SelectedValue))
            {
                using (var client = apiClient())
                {
                    try
                    {
                        await client.SendAsync(User.GetAccessToken(),
                            new SetSiteOfExport(new Guid(model.SelectedValue), model.NotificationId));
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

            return RedirectToAction("Index", "Importer", new { id = model.NotificationId });
        }
    }
}