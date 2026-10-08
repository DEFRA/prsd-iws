namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.CustomsOffice;
    using Core.Notification.Audit;
    using EA.Iws.Core.Notification;
    using EA.Iws.Core.Shared;
    using EA.Iws.Core.TransportRoute;
    using EA.Iws.Requests.Notification;
    using EA.Iws.Requests.TransportRoute;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Requests.CustomsOffice;
    using Requests.Shared;
    using ViewModels.CustomsOffice;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class ExitCustomsOfficeController : Controller
    {
        private readonly IMediator mediator;
        private readonly IAuditService auditService;

        public ExitCustomsOfficeController(IMediator mediator, IAuditService auditService)
        {
            this.mediator = mediator;
            this.auditService = auditService;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, bool? backToOverview = null)
        {
            var data = await mediator.SendAsync(new GetExitCustomsOfficeAddDataByNotificationId(id));

            var existing = await mediator.SendAsync(new GetEntryExitCustomsOfficeSelectionForNotificationById(id));

            if (data.CustomsOffices != CustomsOffices.EntryAndExit
                && data.CustomsOffices != CustomsOffices.Exit)
            {
                return RedirectToAction("Index", "CustomsOffice", new { id });
            }

            CustomsOfficeViewModel model;
            if (data.CustomsOfficeData != null)
            {
                model = new CustomsOfficeViewModel
                {
                    Address = data.CustomsOfficeData.Address,
                    Name = data.CustomsOfficeData.Name,
                    SelectedCountry = data.CustomsOfficeData.Country.Id,
                    Countries = new SelectList(data.Countries, "Id", "Name", data.CustomsOfficeData.Country.Id),
                    Steps = (data.CustomsOffices == CustomsOffices.EntryAndExit) ? 2 : 1
                };
            }
            else
            {
                model = new CustomsOfficeViewModel
                {
                    Countries = new SelectList(data.Countries, "Id", "Name"),
                    Steps = (data.CustomsOffices == CustomsOffices.EntryAndExit) ? 2 : 1
                };
            }

            if (existing == null)
            {
                model.CustomsOfficeRequired = null;
            }
            else
            {
                model.CustomsOfficeRequired = existing.Exit;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, CustomsOfficeViewModel model, bool? backToOverview = null)
        {
            var countries = await mediator.SendAsync(new GetEuropeanUnionCountries());
            var route = await mediator.SendAsync(new GetTransportRouteSummaryForNotification(id));

            model.Countries = model.SelectedCountry.HasValue
                ? new SelectList(countries, "Id", "Name", model.SelectedCountry.Value)
                : new SelectList(countries, "Id", "Name");

            var exitedEU = GetExitedEU(route, countries);
            if (exitedEU && model.SelectedCountry == null)
            {
                ModelState.AddModelError("SelectedCountry", ExitCustomsOfficeResource.EUExit);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingData = await mediator.SendAsync(new GetExitCustomsOfficeAddDataByNotificationId(id));
            NotificationAuditType auditType = NotificationAuditType.Added;

            if (model.CustomsOfficeRequired.GetValueOrDefault())
            {
                await mediator.SendAsync(
                    new SetExitCustomsOfficeForNotificationById(id,
                    model.Name,
                    model.Address,
                    model.SelectedCountry.Value));
                auditType = existingData.CustomsOfficeData == null ? NotificationAuditType.Added : NotificationAuditType.Updated;
            }
            else if (existingData.CustomsOfficeData != null)
            {
                // If customs office required is set to false but there is existing data in the database, delete it
                await mediator.SendAsync(new DeleteExitCustomsOfficeByNotificationId(id));
                auditType = NotificationAuditType.Deleted;
            }

            await this.auditService.AddAuditEntry(this.mediator,
                       id,
                       User.GetUserId(),
                       auditType,
                       NotificationAuditScreenType.CustomsOffice);

            var addSelection = await mediator.SendAsync(new SetExitCustomsOfficeSelectionForNotificationById(id, model.CustomsOfficeRequired.GetValueOrDefault()));

            var notificationCompetentAutority = await mediator.SendAsync(new GetNotificationCompetentAuthority(id));
            if (notificationCompetentAutority.Equals(UKCompetentAuthority.NorthernIreland))
            {
                return RedirectToAction("Index", "EntryCustomsOffice", new { id, backToOverview = backToOverview });
            }

            if (backToOverview.GetValueOrDefault())
            {
                return RedirectToAction("Index", "Home", new { id });
            }

            return RedirectToAction("Index", "Shipment", new { id });
        }

        private bool GetExitedEU(TransportRouteData route, CountryData[] countries)
        {
            var routeEndsInEU = countries.Where(c => c.Name.Equals(route?.StateOfImportData?.Country?.Name)).Any();

            if (route.TransitStatesData != null)
            {
                var countEUTransitStates = route.TransitStatesData.Where(t => countries.Where(c => c.Name.Equals(t.Country.Name)).Any()).Count();
                var countTransitStates = route.TransitStatesData.Count();

                if (!routeEndsInEU && countEUTransitStates > 0)
                {
                    return true;
                }

                if (countEUTransitStates != countTransitStates)
                {
                    // This means we have a mixture of EU and Non-EU transist states and we must work out if the route has exited the EU or not.
                    // This assumes the transit states are in order

                    var foundEU = false;
                    foreach (var transit in route.TransitStatesData)
                    {
                        var isEU = countries.Any(c => c.Name.Equals(transit.Country.Name));

                        if (isEU)
                        {
                            foundEU = true;
                        }
                        else
                        {
                            if (foundEU)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}