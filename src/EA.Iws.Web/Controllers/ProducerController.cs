namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Notification;
    using Requests.Producers;
    using Requests.Registration;
    using Requests.Shared;
    using ViewModels.NotificationApplication;
    using ViewModels.Shared;

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
        public async Task<ActionResult> CopyFromExporter(Guid id, YesNoChoiceViewModel inputModel)
        {
            var model = new ExporterNotifier();

            model.NotificationId = id;

            if (!ModelState.IsValid)
            {
                ViewBag.NotificationId = id;
                return View(inputModel);
            }

            if (inputModel.Choices.SelectedValue.Equals("No"))
            {
                return RedirectToAction("Add", new { id = id, copy = "false" });
            }

            return RedirectToAction("Add", new { id = id, copy = "true" });
        }

        [HttpGet]
        public async Task<ActionResult> Add(Guid id, bool copy)
        {
            var model = new ProducerData
            {
                Address = new AddressData(),
                Business = new BusinessData()
            };

            if (copy)
            {
                // TODO: copy from previous
                using (var client = apiClient())
                {
                    var response = await client.SendAsync(new GetExporterByNotificationId(id));

                    if (response.HasErrors)
                    {
                        // TODO: Error handling
                    }

                    var exporter = response.Result;
                    model.Address = exporter.Address;
                    model.Contact = exporter.Contact;
                    model.Business = new BusinessData
                    {
                        Name = exporter.Name,
                        EntityType = exporter.Type,
                        AdditionalRegistrationNumber = exporter.AdditionalRegistrationNumber
                    };
                    model.Business.BindRegistrationNumber(exporter.RegistrationNumber);
                }
            }

            model.NotificationId = id;

            await BindCountrySelectList();
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(ProducerData model)
        {
            if (!ModelState.IsValid)
            {
                await BindCountrySelectList();
                return View(model);
            }

            using (var client = apiClient())
            {
                var response = await client.SendAsync(User.GetAccessToken(), new AddProducerToNotification(model));

                if (response.HasErrors)
                {
                    this.AddValidationErrorsToModelState(response);
                    await BindCountrySelectList(client);
                    return View(model);
                }
            }

            return RedirectToAction("MultipleProducers", "Producer", new { notificationId = model.NotificationId });
        }

        [HttpGet]
        public async Task<ActionResult> MultipleProducers(Guid notificationId, string errorMessage = "")
        {
            var model = new MultipleProducersViewModel();

            if (!String.IsNullOrEmpty(errorMessage))
            {
                ModelState.AddModelError(string.Empty, errorMessage);
            }

            using (var client = apiClient())
            {
                var response = await client.SendAsync(User.GetAccessToken(), new GetProducersByNotificationId(notificationId));

                if (response.HasErrors)
                {
                    this.AddValidationErrorsToModelState(response);
                    return View(model);
                }
                model.NotificationId = notificationId;
                model.ProducerData = response.Result.ToList();
                model.HasSiteOfExport = model.ProducerData.Exists(p => p.IsSiteOfExport);
            }
            return View("MultipleProducers", model);
        }

        [HttpPost]
        public async Task<ActionResult> MultipleProducers(MultipleProducersViewModel model)
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

            if (Request.IsAjaxRequest())
            {
                return PartialView("ConfirmDeleteProducer", model);
            }
            return View("ConfirmDeleteProducer", model);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteProducer(ProducerData model)
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

        private async Task BindCountrySelectList()
        {
            using (var client = apiClient())
            {
                await BindCountrySelectList(client);
            }
        }

        private async Task BindCountrySelectList(IIwsClient client)
        {
            var response = await client.SendAsync(new GetCountries());

            if (response.HasErrors)
            {
                // TODO: Error handling
            }

            ViewBag.Countries = new SelectList(response.Result, "Id", "Name",
                response.Result.Single(c => c.Name.Equals("United Kingdom", StringComparison.InvariantCultureIgnoreCase)).Id);
        }
    }
}