namespace EA.Iws.Domain.ImportNotificationAssessment
{
    using Core.ComponentRegistration;
    using Core.Shared;
    using EA.Iws.Core.ImportNotificationAssessment;
    using EA.Iws.Core.NotificationAssessment;
    using EA.Iws.Domain.NotificationAssessment;
    using ImportNotification;
    using NotificationApplication;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [AutoRegister]
    public class ImportNotificationChargeCalculator : IImportNotificationChargeCalculator
    {
        private readonly IImportNotificationRepository notificationRepository;
        private readonly IImportNotificationAssessmentRepository notificationAssessmentRepository;
        private readonly IShipmentRepository shipmentRepository;
        private readonly IPricingStructureRepository pricingStructureRepository;
        private readonly IPricingFixedFeeRepository pricingFixedFeeRepository;
        private readonly IInterimStatusRepository interimStatusRepository;
        private readonly INumberOfShipmentsHistotyRepository numberOfShipmentsHistotyRepository;
        private readonly IWasteComponentRepository wasteComponentRepository;
        private readonly IWasteTypeRepository wasteTypeRepository;

        public ImportNotificationChargeCalculator(IImportNotificationRepository notificationRepository,
            IImportNotificationAssessmentRepository notificationAssessmentRepository,
            IShipmentRepository shipmentRepository,
            IPricingStructureRepository pricingStructureRepository,
            IPricingFixedFeeRepository pricingFixedFeeRepository,
            IInterimStatusRepository interimStatusRepository,
            INumberOfShipmentsHistotyRepository numberOfShipmentsHistotyRepository,
            IWasteComponentRepository wasteComponentRepository,
            IWasteTypeRepository wasteTypeRepository)
        {
            this.notificationRepository = notificationRepository;
            this.notificationAssessmentRepository = notificationAssessmentRepository;
            this.shipmentRepository = shipmentRepository;
            this.pricingStructureRepository = pricingStructureRepository;
            this.pricingFixedFeeRepository = pricingFixedFeeRepository;
            this.interimStatusRepository = interimStatusRepository;
            this.numberOfShipmentsHistotyRepository = numberOfShipmentsHistotyRepository;
            this.wasteComponentRepository = wasteComponentRepository;
            this.wasteTypeRepository = wasteTypeRepository;
        }

        public async Task<decimal> GetValue(Guid importNotificationId)
        {
            var shipment = await shipmentRepository.GetByNotificationIdOrDefault(importNotificationId);

            if (shipment == null)
            {
                return 0;
            }

            var numberOfShipments = shipment.NumberOfShipments;

            var largestNumberOfShipments = await numberOfShipmentsHistotyRepository.GetLargestNumberOfShipments(importNotificationId);

            if (largestNumberOfShipments > shipment.NumberOfShipments)
            {
                numberOfShipments = largestNumberOfShipments;
            }

            var notification = await notificationRepository.Get(importNotificationId);

            return await GetPrice(notification, numberOfShipments, await GetInterimStatus(importNotificationId));
        }

        public async Task<decimal> GetValueForNumberOfShipments(Guid importNotificationId, int numberOfShipments)
        {
            var notification = await notificationRepository.Get(importNotificationId);

            return await GetPrice(notification, numberOfShipments, await GetInterimStatus(importNotificationId));
        }

        private async Task<bool> GetInterimStatus(Guid notificationId)
        {
            var interimStatus = await interimStatusRepository.GetByNotificationId(notificationId);

            return interimStatus.IsInterim;
        }

        private async Task<decimal> GetPrice(ImportNotification notification, int numberOfShipments, bool isInterim)
        {
            var pricingStructures = await pricingStructureRepository.Get();

            var notificationAssessment = await notificationAssessmentRepository.GetByNotification(notification.Id);

            var submittedDate = GetNotificationAssessmentSubmittedDate(notificationAssessment);

            var correspondingPricingStructure =
                pricingStructures
                .OrderByDescending(ps => ps.ValidFrom)
                .Where(p => p.CompetentAuthority == notification.CompetentAuthority
                    && p.ValidFrom <= submittedDate
                    && p.Activity.TradeDirection == TradeDirection.Import
                    && p.Activity.NotificationType == notification.NotificationType
                    && p.Activity.IsInterim == isInterim
                    && p.ShipmentQuantityRange.RangeFrom <= numberOfShipments
                    && (p.ShipmentQuantityRange.RangeTo == null || p.ShipmentQuantityRange.RangeTo >= numberOfShipments))
                .FirstOrDefault();

            var price = correspondingPricingStructure.Price;

            //Now from here there's some specifics for the new Charge Matrix from April 2024
            if (notification.CompetentAuthority == Core.Notification.UKCompetentAuthority.England
                && submittedDate.Date >= new DateTime(2024, 04, 01))
            {
                var wasteType = await wasteTypeRepository.GetByNotificationId(notification.Id);
                //Check if fixed fee for WasteCategoryType
                if (wasteType.WasteCategoryType != null)
                {
                    var wasteCategoryfee =
                        await pricingFixedFeeRepository.GetWasteCategoryFee(wasteType.WasteCategoryType.Value, submittedDate);
                    if (wasteCategoryfee != null && wasteCategoryfee.Price > 0)
                    {
                        return wasteCategoryfee.Price;
                    }
                }

                if (notification.CompetentAuthority == Core.Notification.UKCompetentAuthority.England)
                {
                    if (numberOfShipments > 1000)
                    {
                        price = IncreasePriceForOver1000Shipments(correspondingPricingStructure.Price, numberOfShipments);
                    }
                }

                var wasteComponents = await wasteComponentRepository.GetByNotificationId(notification.Id);
                if (wasteComponents != null && wasteComponents.Count() > 0)
                {
                    var wasteComponentFees = await pricingFixedFeeRepository.GetAllWasteComponentFees(submittedDate);
                    if (wasteComponentFees.Count() > 0)
                    {
                        foreach (var wasteComponent in wasteComponents)
                        {
                            price += wasteComponentFees
                            .OrderByDescending(ps => ps.ValidFrom)
                            .FirstOrDefault(wcf => wcf.WasteComponentTypeId == wasteComponent.WasteComponentType).Price;
                        }
                    }
                }
            }

            return price;
        }

        private DateTimeOffset GetNotificationAssessmentSubmittedDate(ImportNotificationAssessment notificationAssessment)
        {
            var submittedStatus = notificationAssessment.StatusChanges.FirstOrDefault(x => x.NewStatus == ImportNotificationStatus.Submitted);
            if (submittedStatus != null)
            {
                return submittedStatus.ChangeDate;
            }
            return DateTimeOffset.Now;
        }

        private decimal IncreasePriceForOver1000Shipments(decimal price, int numberOfShipments)
        {
            int hundreds = (numberOfShipments - 1000) / 100;
            price += (price * (decimal)0.10 * hundreds);

            return price;
        }
    }
}