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
        private readonly Func<Guid, RedirectToRouteResult> intendedShipments;
        private readonly Func<Guid, RedirectToRouteResult> transportRouteSummary;
        private readonly Func<Guid, RedirectToRouteResult> addExitCustomsOffice;
        private readonly Func<Guid, RedirectToRouteResult> addEntryCustomsOffice;

        public CustomsOfficeController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;

            intendedShipments = id => this.RedirectToAction("Index", "Shipment", new { id });
            transportRouteSummary = id => this.RedirectToAction("Summary", "TransportRoute", new { id });
            addExitCustomsOffice = id => this.RedirectToAction("Index", "ExitCustomsOffice", new { id });
            addEntryCustomsOffice = id => this.RedirectToAction("Index", "EntryCustomsOffice", new { id });
        }

        public async Task<ActionResult> Index(Guid id)
        {
            CustomsOfficeCompletionStatus customsOffice;
            using (var client = apiClient())
            {
                customsOffice = await client.SendAsync(User.GetAccessToken(), new GetCustomsCompletionStatusByNotificationId(id));
            }

            switch (customsOffice.CustomsOfficesRequired)
            {
                case CustomsOffices.None:
                    return intendedShipments(id);
                case CustomsOffices.EntryAndExit:
                case CustomsOffices.Exit:
                    return addExitCustomsOffice(id);
                case CustomsOffices.Entry:
                    return addEntryCustomsOffice(id);
                default:
                    return transportRouteSummary(id);
            }
        }
    }
}