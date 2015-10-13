namespace EA.Iws.Web.Areas.Movement.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Carriers;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using ViewModels.Carrier;

    public class CarrierController : Controller
    {
        private readonly IMediator mediator;

        public CarrierController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, int? numberOfCarriers = null)
        {
            var carrierData = await mediator.SendAsync(new GetMovementCarrierDataByMovementId(id));

            if (CheckSelectedCarriersExistWithNullOrValidNumber(carrierData.SelectedCarriers, numberOfCarriers)
                || CheckNumberOfCarriersIsValid(numberOfCarriers))
            {
                var viewModel = new CarrierViewModel
                {
                    MovementId = id,
                    NotificationCarriers = carrierData.NotificationCarriers.ToList(),
                    MeansOfTransportViewModel = await GetMeansOfTransport(id)
                };

                viewModel.SetCarrierSelectLists(carrierData.SelectedCarriers,
                    numberOfCarriers ?? carrierData.SelectedCarriers.Count);

                return View(viewModel);
            }
            return RedirectToAction("NumberOfCarriers", "Carrier", new { id });
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

        private async Task<MeansOfTransportViewModel> GetMeansOfTransport(Guid id)
        {
            var meansOfTransport = await mediator.SendAsync(new GetMeansOfTransportByMovementId(id));

            return new MeansOfTransportViewModel
            {
                NotificationMeansOfTransport = meansOfTransport.ToList()
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, CarrierViewModel viewModel, int? numberOfCarriers = null)
        {
            if (!ModelState.IsValid)
            {
                viewModel.MeansOfTransportViewModel = await GetMeansOfTransport(id);

                var carrierData = await mediator.SendAsync(new GetMovementCarrierDataByMovementId(id));
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
                selectedCarriers.Add(i, viewModel.SelectedItems[i].GetValueOrDefault());
            }

            await mediator.SendAsync(new SetActualMovementCarriers(id, selectedCarriers));

            var notificationId = await mediator.SendAsync(new GetNotificationIdByMovementId(id));

                return RedirectToAction("Index", "Submit", new { id, area = "Movement" });
        }

        [HttpGet]
        public async Task<ActionResult> NumberOfCarriers(Guid id)
        {
            var numberOfCarriers = await mediator.SendAsync(new GetNumberOfCarriersByMovementId(id));

            var viewModel = new NumberOfCarriersViewModel
            {
                Amount = numberOfCarriers,
                MeansOfTransportViewModel = await GetMeansOfTransport(id)
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NumberOfCarriers(Guid id, NumberOfCarriersViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.MeansOfTransportViewModel = await GetMeansOfTransport(id);
                return View(viewModel);
            }

            return RedirectToAction("Index", "Carrier", new { id, numberOfCarriers = viewModel.Amount });
        }
    }
}