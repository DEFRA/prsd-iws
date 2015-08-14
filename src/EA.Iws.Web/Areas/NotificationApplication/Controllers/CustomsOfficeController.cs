namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Core.CustomsOffice;
    using Infrastructure;
    using Requests.CustomsOffice;

    public class CustomsOfficeController : Controller
    {
        private readonly Func<IIwsClient> apiClient;
        private readonly Func<Guid, bool?, RedirectToRouteResult> intendedShipments;
        private readonly Func<Guid, bool?, RedirectToRouteResult> transportRouteSummary;
        private readonly Func<Guid, bool?, RedirectToRouteResult> addExitCustomsOffice;
        private readonly Func<Guid, bool?, RedirectToRouteResult> addEntryCustomsOffice;

        public CustomsOfficeController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;

            intendedShipments = (id, backToOverview) => this.RedirectToAction("Index", "Shipment", new { id, backToOverview });
            transportRouteSummary = (id, backToOverview) => this.RedirectToAction("Summary", "TransportRoute", new { id, backToOverview });
            addExitCustomsOffice = (id, backToOverview) => this.RedirectToAction("Index", "ExitCustomsOffice", new { id, backToOverview });
            addEntryCustomsOffice = (id, backToOverview) => this.RedirectToAction("Index", "EntryCustomsOffice", new { id, backToOverview });
        }

        public async Task<ActionResult> Index(Guid id, bool? backToOverview = null)
        {
            CustomsOfficeCompletionStatus customsOffice;
            using (var client = apiClient())
            {
                customsOffice = await client.SendAsync(User.GetAccessToken(), new GetCustomsCompletionStatusByNotificationId(id));
            }

            switch (customsOffice.CustomsOfficesRequired)
            {
                case CustomsOffices.None:
                    return intendedShipments(id, backToOverview);
                case CustomsOffices.EntryAndExit:
                case CustomsOffices.Exit:
                    return addExitCustomsOffice(id, backToOverview);
                case CustomsOffices.Entry:
                    return addEntryCustomsOffice(id, backToOverview);
                default:
                    return transportRouteSummary(id, backToOverview);
            }
        }
    }
}