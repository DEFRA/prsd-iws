namespace EA.Iws.Web.Areas.AddressBook.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.AddressBook;
    using Prsd.Core.Mediator;
    using Requests.AddressBook;

    public class DeleteController : Controller
    {
        private readonly IMediator mediator;

        public DeleteController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<ActionResult> Index(Guid id, AddressRecordType type)
        {
            var result = await mediator.SendAsync(new GetAddressBookRecordById(id, type));

            return View(result);
        } 
    }
}