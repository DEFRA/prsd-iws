namespace EA.Iws.Web.Areas.AdminImportAssessment.Controllers
{
    using System;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using ViewModels;

    [Authorize(Roles = "internal")]
    public class KeyDatesController : Controller
    {
        private readonly IMediator mediator;

        public KeyDatesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult Index(Guid id)
        {
            var model = new DateInputViewModel();

            return View(model);
        }
    }
}