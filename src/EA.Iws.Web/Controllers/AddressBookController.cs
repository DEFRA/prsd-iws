namespace EA.Iws.Web.Controllers
{
    using System.Net.Mime;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.AddressBook;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Prsd.Core.Mediator;
    using Requests.AddressBook;

    [Authorize]
    public class AddressBookController : Controller
    {
        private const string JsonContentType = "application/json";

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

        [HttpGet]
        public async Task<ActionResult> Search(string term, AddressRecordType type)
        {
            if (term != null)
            {
                var result = await mediator.SendAsync(new SearchAddressRecords(term, type));
                return Content(JsonConvert.SerializeObject(result, new JsonSerializerSettings
                {
                    Converters = new JsonConverter[] { new StringEnumConverter() }
                }), JsonContentType);
            }

            return Content(JsonConvert.SerializeObject(new AddressBookRecordData[0]), JsonContentType);
        }
    }
}