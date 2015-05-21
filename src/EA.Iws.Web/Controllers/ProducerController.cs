namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Exporters;
    using Requests.Producers;
    using Requests.Shared;
    using ViewModels.Producer;
    using ViewModels.Shared;

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
        public ActionResult CopyFromExporter(Guid id, YesNoChoiceViewModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.NotificationId = id;
                return View(inputModel);
            }

            if (inputModel.Choices.SelectedValue.Equals("No"))
            {
                return RedirectToAction("Add", new { id });
            }

            return RedirectToAction("Add", new { id, copy = true });
        }

        [HttpGet]
        public async Task<ActionResult> Add(Guid id, bool? copy)
        {
            var model = new ProducerViewModel();

            if (copy.HasValue && copy.Value)
            {
                using (var client = apiClient())
                {
                    var exporter = await client.SendAsync(new GetExporterByNotificationId(id));

                    model.Address = exporter.Address;
                    model.Contact = exporter.Contact;
                    model.Business = (BusinessViewModel)exporter.Business;
                }
            }

            model.NotificationId = id;

            await this.BindCountryList(apiClient);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(ProducerViewModel model)
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
                    var producer = new ProducerData
                    {
                        NotificationId = model.NotificationId,
                        Address = model.Address,
                        Business = (BusinessData)model.Business,
                        Contact = model.Contact,
                        IsSiteOfExport = model.IsSiteOfExport
                    };

                    var response =
                        await client.SendAsync(User.GetAccessToken(), new AddProducerToNotification(producer));

                    return RedirectToAction("MultipleProducers", "Producer",
                        new { notificationId = model.NotificationId });
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
        public async Task<ActionResult> MultipleProducers(Guid notificationId)
        {
            var model = new MultipleProducersViewModel();

            using (var client = apiClient())
            {
                var response =
                    await client.SendAsync(User.GetAccessToken(), new GetProducersByNotificationId(notificationId));

                try
                {
                    model.NotificationId = notificationId;
                    model.ProducerData = response.ToList();
                    model.HasSiteOfExport = model.ProducerData.Exists(p => p.IsSiteOfExport);
                    return View("MultipleProducers", model);
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
    }
}