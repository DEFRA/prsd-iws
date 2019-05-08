namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using ViewModels.EditContact;

    public class EditContactController : Controller
    {
        private readonly IMediator mediator;
        private readonly IAuditService auditService;

        public EditContactController(IMediator mediator, IAuditService auditService)
        {
            this.mediator = mediator;
            this.auditService = auditService;
        }

        [HttpGet]
        public ActionResult Exporter(Guid id)
        {           
            var model = new EditContactViewModel();
            return View(model);
        }

        [HttpGet]
        public ActionResult Importer(Guid id)
        {
            var model = new EditContactViewModel();
            return View(model);
        }
    }
}