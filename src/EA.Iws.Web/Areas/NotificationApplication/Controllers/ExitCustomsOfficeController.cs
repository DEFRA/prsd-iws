namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.CustomsOffice;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Requests.CustomsOffice;
    using Requests.Shared;
    using ViewModels.CustomsOffice;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class ExitCustomsOfficeController : Controller
    {
        private readonly IMediator mediator;

        public ExitCustomsOfficeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var data = await mediator.SendAsync(new GetExitCustomsOfficeAddDataByNotificationId(id));

            if (data.CustomsOffices != CustomsOffices.EntryAndExit
                && data.CustomsOffices != CustomsOffices.Exit)
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
                    Steps = (data.CustomsOffices == CustomsOffices.EntryAndExit) ? 2 : 1
                };
            }
            else
            {
                model = new CustomsOfficeViewModel
                {
                    Countries = new SelectList(data.Countries, "Id", "Name"),
                    Steps = (data.CustomsOffices == CustomsOffices.EntryAndExit) ? 2 : 1
                };
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, CustomsOfficeViewModel model)
        {
            var countries = await mediator.SendAsync(new GetEuropeanUnionCountries());

            model.Countries = model.SelectedCountry.HasValue
                ? new SelectList(countries, "Id", "Name", model.SelectedCountry.Value)
                : new SelectList(countries, "Id", "Name");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            CustomsOfficeCompletionStatus result = await mediator.SendAsync(
                new SetExitCustomsOfficeForNotificationById(id,
                model.Name,
                model.Address,
                model.SelectedCountry.Value));

            switch (result.CustomsOfficesRequired)
            {
                case CustomsOffices.EntryAndExit:
                    return RedirectToAction("Index", "EntryCustomsOffice", new { id });
                default:
                    return RedirectToAction("Index", "Shipment", new { id });
            }
        }
    }
}