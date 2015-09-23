namespace EA.Iws.Web.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.AddressBook;

    public class AddressBookController : Controller
    {
        private readonly IMediator mediator;

        public AddressBookController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var result = await mediator.SendAsync(new GetProducerAddressBook());

            return View(result);
        }
    }
}