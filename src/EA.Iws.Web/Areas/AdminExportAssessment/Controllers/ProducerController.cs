namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;
    using ViewModels.Producer;

    [AuthorizeActivity(typeof(AddProducer))]
    public class ProducerController : Controller
    {
        private readonly IMediator mediator;

        public ProducerController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Add(Guid id)
        {
            var model = new AddProducerViewModel
            {
                NotificationId = id
            };

            await this.BindCountryList(mediator);
            model.Address.DefaultCountryId = this.GetDefaultCountryId();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(Guid id, AddProducerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await this.BindCountryList(mediator);
                model.Address.DefaultCountryId = this.GetDefaultCountryId();

                return View(model);
            }

            var request = new AddProducer
            {
                NotificationId = id,
                Address = model.Address,
                Business = model.Business.ToBusinessInfoData(),
                Contact = model.Contact
            };

            await mediator.SendAsync(request);

            return RedirectToAction("Index", "Overview");
        }
    }
}