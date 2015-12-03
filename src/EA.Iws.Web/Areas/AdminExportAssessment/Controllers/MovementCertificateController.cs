namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using ViewModels;

    [Authorize(Roles = "internal")]
    public class MovementCertificateController : Controller
    {
        private readonly IMediator mediator;

        public MovementCertificateController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public ActionResult Index(Guid id)
        {
            return View(new MovementSearchViewModel());
        }

        public ActionResult Search(MovementSearchViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            throw new NotImplementedException();
        } 
    }
}