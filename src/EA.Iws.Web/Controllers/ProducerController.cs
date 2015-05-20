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
    using Requests.Registration;
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
                return RedirectToAction("Add", new { id = id });
            }

            return RedirectToAction("Add", new { id, copy = true });
        }

        [HttpGet]
        public async Task<ActionResult> Add(Guid id, bool? copy)
        {
            var model = new ProducerViewModel
            {
                Address = new AddressData(),
                Contact = new ContactData(),
                Business = new BusinessViewModel()
            };

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
                    NotificationId = model.NotificationId,
                    Address = model.Address,
                    Business = (BusinessData)model.Business,
                    Contact = model.Contact,
                    IsSiteOfExport = model.IsSiteOfExport
                };

                var response = await client.SendAsync(User.GetAccessToken(), new AddProducerToNotification(producer));

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

                await BindCountrySelectList(client);
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

        [HttpPost]
        public ActionResult MultipleProducers(MultipleProducersViewModel model)
        {
            if (!model.HasSiteOfExport)
            {
                return RedirectToAction("MultipleProducers", "Producer",
                    new
                    {
                        notificationId = model.NotificationId,
                        errorMessage = "Please select a site of export"
                    });
            }
            return RedirectToAction("Add", "Importer", new { id = model.NotificationId });
        }

        [HttpGet]
        public ActionResult ShowConfirmDelete(Guid producerId, Guid notificationId, bool isSiteOfExport)
        {
            var model = new ProducerData
            {
                Id = producerId,
                NotificationId = notificationId,
                IsSiteOfExport = isSiteOfExport
            };

            ViewBag.Countries = new SelectList(response, "Id", "Name",
                response.Single(c => c.Name.Equals("United Kingdom", StringComparison.InvariantCultureIgnoreCase)).Id);
            }
            return View("ConfirmDeleteProducer", model);
        {
            if (model.IsSiteOfExport)
            {
                return RedirectToAction("MultipleProducers", "Producer",
                    new
                    {
                        notificationId = model.NotificationId,
                        errorMessage = "Please make another producer the site of export before you delete this producer"
                    });
            }
            using (var client = apiClient())
            {
                await client.SendAsync(User.GetAccessToken(), new DeleteProducer(model.Id, model.NotificationId));
            }
            return RedirectToAction("MultipleProducers", "Producer", new { notificationId = model.NotificationId });
        }
    }
}