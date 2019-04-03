namespace EA.Iws.Web.Areas.AddressBook.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using AddressBook.ViewModels;
    using Core.AddressBook;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.AddressBook;

    [AuthorizeActivity(typeof(EditAddressBookEntry))]
    public class EditController : Controller
    {
        private readonly IMediator mediator;

        public EditController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, AddressRecordType type, int page = 1)
        {
            var result = await mediator.SendAsync(new GetAddressBookRecordById(id, type));

            ViewBag.Type = type;
            var model = new EditAddressViewModel(result, type, page);

            await this.BindCountryList(mediator);
            model.Address.DefaultCountryId = this.GetDefaultCountryId();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(EditAddressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await this.BindCountryList(mediator);
                return View(model);
            }

            try
            {
                await mediator.SendAsync(new EditAddressBookEntry(model.AddressBookRecordId, model.Type, model.Address, model.Business.ToBusinessInfoData(), model.Contact));
                                
                return RedirectToAction("Index", "Home", new { type = model.Type, page = model.PageNumber });
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
        public async Task<ActionResult> Add(AddressRecordType type)
        {
            ViewBag.Type = type;
            var model = new AddAddressViewModel();
            model.Type = type;
            await this.BindCountryList(mediator);
            model.Address.DefaultCountryId = this.GetDefaultCountryId();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(AddAddressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await this.BindCountryList(mediator);
                return View(model);
            }

            try
            {
                await mediator.SendAsync(
                    new AddNewAddressBookEntry
                    {
                        Address = model.Address,
                        Business = model.Business.ToBusinessInfoData(),
                        Contact = model.Contact,
                        Type = model.Type
                    });

                return RedirectToAction("Index", "Home", new { type = model.Type });
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
    }
}