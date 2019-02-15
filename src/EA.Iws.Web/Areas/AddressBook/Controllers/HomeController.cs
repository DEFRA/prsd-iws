namespace EA.Iws.Web.Areas.AddressBook.Controllers
{
    using System.Linq;
    using System.Security;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.AddressBook;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.AddressBook;

    [AuthorizeActivity(typeof(GetUserAddressBookByType))]
    public class HomeController : Controller
    {
        private readonly IMediator mediator;

        public HomeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(AddressRecordType? type)
        {
            var result = await mediator.SendAsync(new GetUserAddressBookByType(type ?? AddressRecordType.Producer));

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(AddressBookData model, string button, int page = 1)
        {
            if (button == "home")
            {
                return RedirectToAction("Home", "Applicant", new { area = string.Empty });
            }

            if (button == "search")
            {
                if (!string.IsNullOrWhiteSpace(model.SearchTerm))
                {
                    var result = await mediator.SendAsync(new SearchAddressRecordsByName(model.SearchTerm, model.Type));

                    model.AddressRecords = result.AddressRecords;

                    return View(model);
                }
            }

            return RedirectToAction("Index", new { model.Type });
        }
    }
}