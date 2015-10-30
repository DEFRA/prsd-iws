namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System;
    using System.Web.Mvc;
    using Core.ImportNotification.Draft;
    using Prsd.Core.Mapper;
    using ViewModels.Shipment;

    [Authorize(Roles = "internal")]
    public class ShipmentController : Controller
    {
        private readonly IMapper mapper;

        public ShipmentController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        [HttpGet]
        public ActionResult Index(Guid id)
        {
            var model = new ShipmentViewModel
            {
                ImportNotificationId = id
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Guid id, ShipmentViewModel model)
        {
            var data = mapper.Map<Shipment>(model);

            return HttpNotFound();
        }
    }
}