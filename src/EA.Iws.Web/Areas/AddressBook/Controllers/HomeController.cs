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
    using Requests.Users;

    [AuthorizeActivity(typeof(GetUserAddressBookByType))]
    public class HomeController : Controller
    {
        private readonly IMediator mediator;

        public HomeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(AddressRecordType? type, string searchTerm, int page = 1)
        {
            AddressBookData model = new AddressBookData();
            AddressBookData result = null;

            var isInternalUser = await mediator.SendAsync(new GetUserIsInternal());
            model.IsInternalUser = isInternalUser;

            if (searchTerm == null)
            {
                result = await mediator.SendAsync(new GetUserAddressBookByType(type ?? AddressRecordType.Producer, page));
            }
            else if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                result = await mediator.SendAsync(new SearchAddressRecordsByName(searchTerm, type ?? AddressRecordType.Producer, page));
            }

            if (result != null)
            {
                model.AddressRecords = result.AddressRecords;
                model.PageNumber = result.PageNumber;
                model.PageSize = result.PageSize;
                model.NumberOfMatchedRecords = result.NumberOfMatchedRecords;
                model.SearchTerm = searchTerm;
                model.Type = type ?? AddressRecordType.Producer;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string button, AddressBookData model, int page = 1)
        {
            if (button == "home")
            {
                return RedirectToAction("Home", "Applicant", new { area = string.Empty });
            }

            if (button == "search")
            {
                return RedirectToAction("Index", new { model.Type, searchTerm = model.SearchTerm });
            }

            return RedirectToAction("Index", new { model.Type });
        }
    }
}