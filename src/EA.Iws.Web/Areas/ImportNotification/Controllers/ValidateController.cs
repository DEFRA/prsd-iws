namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.AddressBook;
    using Requests.ImportNotification;
    using Requests.ImportNotification.Validate;
    using ViewModels.Validate;

    [AuthorizeActivity(typeof(CompleteDraftImportNotification))]
    public class ValidateController : Controller
    {
        private readonly IMediator mediator;

        public ValidateController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var results = await mediator.SendAsync(new ValidateImportNotification(id));

            var model = new ValidateViewModel(results);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(Guid id)
        {
            var result = await mediator.SendAsync(new CompleteDraftImportNotification(id));

            if (result)
            {
                var addresses = await mediator.SendAsync(new AddImportAddressBookEntry(id));
                return RedirectToAction("Index", "Complete");
            }

            return RedirectToAction("Error", "Complete");
        }
    }
}