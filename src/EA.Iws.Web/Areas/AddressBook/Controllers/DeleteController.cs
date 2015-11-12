namespace EA.Iws.Web.Areas.AddressBook.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.AddressBook;
    using Prsd.Core.Mediator;
    using Requests.AddressBook;

    [Authorize]
    public class DeleteController : Controller
    {
        private readonly IMediator mediator;

        public DeleteController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, AddressRecordType type)
        {
            var result = await mediator.SendAsync(new GetAddressBookRecordById(id, type));

            ViewBag.Type = type;

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, AddressRecordType type, FormCollection model)
        {
            await mediator.SendAsync(new DeleteAddressBookRecord(id, type));

            return RedirectToAction("Index", "Home", new { type });
        } 
    }
}