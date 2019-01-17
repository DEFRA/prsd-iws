namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System.Web.Mvc;
    using Prsd.Core.Mediator;

    public class PrenotificationBulkUploadController : Controller
    {
        private readonly IMediator mediator;

        public PrenotificationBulkUploadController(IMediator mediator)
        {
            this.mediator = mediator;
        }
    }
}