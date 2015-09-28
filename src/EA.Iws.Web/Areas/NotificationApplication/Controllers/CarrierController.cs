namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Carriers;
    using Infrastructure;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.AddressBook;
    using Requests.Carriers;
    using ViewModels.Carrier;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class CarrierController : Controller
    {
        private readonly IMediator mediator;
        private readonly IMap<AddCarrierViewModel, AddAddressBookEntry> addCarrierAddressBookMap;

        public CarrierController(IMediator mediator, IMap<AddCarrierViewModel, AddAddressBookEntry> addCarrierAddressBookMap)
        {
            this.mediator = mediator;
            this.addCarrierAddressBookMap = addCarrierAddressBookMap;
        }

        [HttpGet]
        public async Task<ActionResult> Add(Guid id, bool? backToOverview = null)
        {
            ViewBag.BackToOverview = backToOverview.GetValueOrDefault();
            var model = new AddCarrierViewModel { NotificationId = id };
            await this.BindCountryList(mediator);
            model.Address.DefaultCountryId = this.GetDefaultCountryId();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(AddCarrierViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                await this.BindCountryList(mediator);
                return View(model);
            }
            try
            {
                await mediator.SendAsync(model.ToRequest());

                if (model.IsAddedToAddressBook)
                {
                    await mediator.SendAsync(addCarrierAddressBookMap.Map(model));
                }

                return RedirectToAction("List", "Carrier", new { id = model.NotificationId, backToOverview });
            }
            catch (ApiBadRequestException ex)
            {
                this.HandleBadRequest(ex);
                if (ModelState.IsValid)
                {
                    throw;
                }
            }

            await this.BindCountryList(mediator);
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(Guid id, Guid entityId, bool? backToOverview = null)
        {
            var carrier = await mediator.SendAsync(new GetCarrierForNotification(id, entityId));

            var model = new EditCarrierViewModel(carrier);

            await this.BindCountryList(mediator);
            model.Address.DefaultCountryId = this.GetDefaultCountryId();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditCarrierViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                await this.BindCountryList(mediator);
                return View(model);
            }

            try
            {
                var request = model.ToRequest();

                await mediator.SendAsync(request);

                if (model.IsAddedToAddressBook)
                {
                    await mediator.SendAsync(addCarrierAddressBookMap.Map(model));
                }

                return RedirectToAction("List", "Carrier",
                    new { id = model.NotificationId, backToOverview });
            }
            catch (ApiBadRequestException ex)
            {
                this.HandleBadRequest(ex);

                if (ModelState.IsValid)
                {
                    throw;
                }
            }
            await this.BindCountryList(mediator);
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> List(Guid id, bool? backToOverview = null)
        {
            ViewBag.BackToOverview = backToOverview.GetValueOrDefault();
            var model = new CarrierListViewModel();

            var response =
                await mediator.SendAsync(new GetCarriersByNotificationId(id));

            model.NotificationId = id;
            model.Carriers = new List<CarrierData>(response);

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Remove(Guid id, Guid entityId, bool? backToOverview = null)
        {
            ViewBag.BackToOverview = backToOverview.GetValueOrDefault();

            var carrier = await mediator.SendAsync(new GetCarrierForNotification(id, entityId));

            var model = new RemoveCarrierViewModel
            {
                CarrierId = carrier.Id,
                CarrierName = carrier.Business.Name,
                NotificationId = carrier.NotificationId
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Remove(RemoveCarrierViewModel model, bool? backToOverview = null)
        {
            try
            {
                await mediator.SendAsync(new DeleteCarrierForNotification(model.NotificationId, model.CarrierId));
            }
            catch (ApiBadRequestException ex)
            {
                this.HandleBadRequest(ex);
                if (ModelState.IsValid)
                {
                    throw;
                }
            }

            return RedirectToAction("List", "Carrier", new { id = model.NotificationId, backToOverview });
        }
    }
}