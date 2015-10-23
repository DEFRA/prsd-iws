namespace EA.Iws.Web.Areas.NotificationAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using ViewModels;

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

        public async Task<ActionResult> Search(MovementSearchViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            throw new NotImplementedException();
        } 
    }
}