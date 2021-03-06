﻿namespace EA.Iws.Web.Areas.AddressBook.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.AddressBook;
    using Infrastructure.Authorization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Prsd.Core.Mediator;
    using Requests.AddressBook;

    [AuthorizeActivity(typeof(SearchAddressRecords))]
    public class SearchController : Controller
    {
        private const string JsonContentType = "application/json";

        private readonly IMediator mediator;

        public SearchController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(string term, AddressRecordType type)
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