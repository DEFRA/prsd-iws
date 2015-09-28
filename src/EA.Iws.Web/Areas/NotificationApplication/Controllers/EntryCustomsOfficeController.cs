namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Core.CustomsOffice;
    using Infrastructure;
    using Requests.CustomsOffice;
    using Requests.Shared;
    using ViewModels.CustomsOffice;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class EntryCustomsOfficeController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public EntryCustomsOfficeController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            using (var client = apiClient())
            {
                var data = await client.SendAsync(User.GetAccessToken(), new GetEntryCustomsOfficeAddDataByNotificationId(id));

                if (data.CustomsOfficesRequired != CustomsOffices.EntryAndExit
                    && data.CustomsOfficesRequired != CustomsOffices.Entry)
                {
                    return RedirectToAction("Index", "CustomsOffice", new { id });
                }

                CustomsOfficeViewModel model;
                if (data.CustomsOfficeData != null)
                {
                    model = new CustomsOfficeViewModel
                    {
                        Address = data.CustomsOfficeData.Address,
                        Name = data.CustomsOfficeData.Name,
                        SelectedCountry = data.CustomsOfficeData.Country.Id,
                        Countries = new SelectList(data.Countries, "Id", "Name", data.CustomsOfficeData.Country.Id),
                        Steps = (data.CustomsOfficesRequired == CustomsOffices.EntryAndExit) ? 2 : 1
                    };
                }
                else
                {
                    model = new CustomsOfficeViewModel
                    {
                        Countries = new SelectList(data.Countries, "Id", "Name"),
                        Steps = (data.CustomsOfficesRequired == CustomsOffices.EntryAndExit) ? 2 : 1
                    };
                }

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, CustomsOfficeViewModel model)
        {
            using (var client = apiClient())
            {
                var countries = await client.SendAsync(User.GetAccessToken(), new GetEuropeanUnionCountries());

                model.Countries = model.SelectedCountry.HasValue
                    ? new SelectList(countries, "Id", "Name", model.SelectedCountry.Value)
                    : new SelectList(countries, "Id", "Name");

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                await client.SendAsync(User.GetAccessToken(), new SetEntryCustomsOfficeForNotificationById(id,
                    model.Name,
                    model.Address,
                    model.SelectedCountry.Value));

                return RedirectToAction("Index", "Shipment", new { id });
            }
        }
    }
}