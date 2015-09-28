namespace EA.Iws.Web.Areas.AddressBook.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.AddressBook;
    using Prsd.Core.Mediator;
    using Requests.AddressBook;

    [Authorize]
    public class HomeController : Controller
    {
        private readonly IMediator mediator;

        public HomeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<ActionResult> Index(AddressRecordType? type)
        {
            var result = await mediator.SendAsync(new GetUserAddressBookByType(type ?? AddressRecordType.Producer));

            return View(result);
        }
    }
}