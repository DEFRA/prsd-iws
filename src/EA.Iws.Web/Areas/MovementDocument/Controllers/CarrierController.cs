namespace EA.Iws.Web.Areas.MovementDocument.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Core.Carriers;
    using Infrastructure;
    using Requests.MeansOfTransport;
    using Requests.Movement;
    using ViewModels.Carrier;

    public class CarrierController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public CarrierController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid movementId, Guid notificationId, int? numberOfCarriers = null)
        {
            using (var client = apiClient())
            {
                var carrierData = await client.SendAsync(User.GetAccessToken(), new GetMovementCarrierDataByMovementId(movementId));

                if (CheckSelectedCarriersExistWithNullOrValidNumber(carrierData.SelectedCarriers, numberOfCarriers)
                    || CheckNumberOfCarriersIsValid(numberOfCarriers))
                {
                    var viewModel = new CarrierViewModel
                    {
                        MovementId = movementId,
                        NotificationCarriers = carrierData.NotificationCarriers.ToList(),
                        MeansOfTransportViewModel = await GetMeansOfTransport(client, notificationId)
                    };

                    viewModel.SetCarrierSelectLists(carrierData.SelectedCarriers,
                        numberOfCarriers ?? carrierData.SelectedCarriers.Count);

                    return View(viewModel);
                }
                else
                {
                    return RedirectToAction("NumberOfCarriers", "Carrier", new { movementId });
                }
            }
        }

        private bool CheckSelectedCarriersExistWithNullOrValidNumber(Dictionary<int, CarrierData> selectedCarriers, int? numberOfCarriers)
        {
            return CheckSelectedCarriersExist(selectedCarriers)
                && (!numberOfCarriers.HasValue || CheckNumberOfCarriersIsValid(numberOfCarriers));
        }

        private bool CheckSelectedCarriersExist(Dictionary<int, CarrierData> selectedCarriers)
        {
            return selectedCarriers != null
                && selectedCarriers.Any();
        }

        private bool CheckNumberOfCarriersIsValid(int? numberOfCarriers)
        {
            return numberOfCarriers.HasValue
                && numberOfCarriers.Value > 0
                && numberOfCarriers.Value < 101;
        }

        private async Task<MeansOfTransportViewModel> GetMeansOfTransport(IIwsClient client, Guid notificationId)
        {
            var meansOfTransport = await client.SendAsync(User.GetAccessToken(),
                 new GetMeansOfTransportByNotificationId(notificationId));

            return new MeansOfTransportViewModel
            {
                NotificationMeansOfTransport = meansOfTransport.ToList()
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid movementId, Guid notificationId, CarrierViewModel viewModel, int? numberOfCarriers = null)
        {
            using (var client = apiClient())
            {
                if (!ModelState.IsValid)
                {
                    viewModel.MeansOfTransportViewModel = await GetMeansOfTransport(client, notificationId);

                    var carrierData = await client.SendAsync(User.GetAccessToken(), new GetMovementCarrierDataByMovementId(movementId));
                    viewModel.NotificationCarriers = carrierData.NotificationCarriers.ToList();

                    if (viewModel.SelectedItems != null)
                    {
                        viewModel.SetCarrierSelectListsFromSelectedValues();
                    }

                    return View(viewModel);
                }

                var selectedCarriers = new Dictionary<int, Guid>();

                for (int i = 0; i < viewModel.SelectedItems.Count; i++)
                {
                    selectedCarriers.Add(i, viewModel.SelectedItems[i].Value);
                }

                await client.SendAsync(User.GetAccessToken(), new SetActualMovementCarriers(movementId, selectedCarriers));

                return RedirectToAction("Index", "Home", new { movementId });
            }
        }

        [HttpGet]
        public async Task<ActionResult> NumberOfCarriers(Guid movementId, Guid notificationId)
        {
            using (var client = apiClient())
            {
                var numberOfCarriers = await client.SendAsync(User.GetAccessToken(),
                    new GetNumberOfCarriersByMovementId(movementId));

                var viewModel = new NumberOfCarriersViewModel
                {
                    Amount = numberOfCarriers,
                    MeansOfTransportViewModel = await GetMeansOfTransport(client, notificationId)
                };

                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NumberOfCarriers(Guid movementId, Guid notificationId, NumberOfCarriersViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                using (var client = apiClient())
                {
                    viewModel.MeansOfTransportViewModel = await GetMeansOfTransport(client, notificationId);
                    return View(viewModel);
                }
            }

            return RedirectToAction("Index", "Carrier", new { movementId, numberOfCarriers = viewModel.Amount });
        }
    }
}