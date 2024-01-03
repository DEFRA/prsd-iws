namespace EA.Iws.Domain.NotificationApplication
{
    using Core.ComponentRegistration;
    using EA.Iws.Core.NotificationAssessment;
    using EA.Iws.Domain.NotificationAssessment;
    using Shipment;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [AutoRegister]
    public class NotificationChargeCalculator : INotificationChargeCalculator
    {
        private readonly IShipmentInfoRepository shipmentInfoRepository;
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly IPricingStructureRepository pricingStructureRepository;
        private readonly IPricingFixedFeeRepository pricingFixedFeeRepository;
        private readonly IFacilityRepository facilityRepository;
        private readonly INumberOfShipmentsHistotyRepository numberOfShipmentsHistotyRepository;
        private readonly INotificationAssessmentRepository notificationAssessmentRepository;

        public NotificationChargeCalculator(IShipmentInfoRepository shipmentInfoRepository,
            INotificationApplicationRepository notificationApplicationRepository,
            IPricingStructureRepository pricingStructureRepository,
            IPricingFixedFeeRepository pricingFixedFeeRepository,
            IFacilityRepository facilityRepository,
            INumberOfShipmentsHistotyRepository numberOfShipmentsHistotyRepository,
            INotificationAssessmentRepository notificationAssessmentRepository)
        {
            this.shipmentInfoRepository = shipmentInfoRepository;
            this.notificationApplicationRepository = notificationApplicationRepository;
            this.pricingStructureRepository = pricingStructureRepository;
            this.pricingFixedFeeRepository = pricingFixedFeeRepository;
            this.facilityRepository = facilityRepository;
            this.numberOfShipmentsHistotyRepository = numberOfShipmentsHistotyRepository;
            this.notificationAssessmentRepository = notificationAssessmentRepository;
        }

        public async Task<decimal> GetValue(Guid notificationId)
        {
            var shipmentInfo = await shipmentInfoRepository.GetByNotificationId(notificationId);

            if (shipmentInfo == null)
            {
                return 0;
            }

            var numberOfShipments = shipmentInfo.NumberOfShipments;

            var largestNumberOfShipments = await numberOfShipmentsHistotyRepository.GetLargestNumberOfShipments(notificationId);

            if (largestNumberOfShipments > shipmentInfo.NumberOfShipments)
            {
                numberOfShipments = largestNumberOfShipments;
            }

            var notification = await notificationApplicationRepository.GetById(notificationId);

            return await GetPrice(numberOfShipments, notification);
        }

        public async Task<decimal> GetValueForNumberOfShipments(Guid notificationId, int numberOfShipments)
        {
            var notification = await notificationApplicationRepository.GetById(notificationId);

            return await GetPrice(numberOfShipments, notification);
        }

        private async Task<decimal> GetPrice(int numberOfShipments, NotificationApplication notification)
        {
            var facilityCollection = await facilityRepository.GetByNotificationId(notification.Id);
            var notificationAssessment = await notificationAssessmentRepository.GetByNotificationId(notification.Id);

            var submittedDate = GetNotificationAssessmentSubmittedDate(notificationAssessment);

            bool isInterim = facilityCollection.IsInterim.HasValue ? facilityCollection.IsInterim.Value : facilityCollection.HasMultipleFacilities;

            var result = await pricingStructureRepository.GetExport(notification.CompetentAuthority,
                notification.NotificationType,
                numberOfShipments,
                isInterim,
                submittedDate);

            var price = result.Price;

            //Now from here there's some specifics for the new Charge Matrix from April 2024
            if (notification.CompetentAuthority == Core.Notification.UKCompetentAuthority.England 
                && submittedDate.Date >= new DateTime(2024, 04, 01))
            {
                //Check if fixed fee for WasteCategoryType
                if (notification.WasteType.WasteCategoryType != null)
                {
                    var wasteCategoryfee =
                        await pricingFixedFeeRepository.GetWasteCategoryFee(notification.WasteType.WasteCategoryType.Value, submittedDate);
                    if (wasteCategoryfee != null && wasteCategoryfee.Price > 0)
                    {
                        return wasteCategoryfee.Price;
                    }
                }

                if (notification.CompetentAuthority == Core.Notification.UKCompetentAuthority.England)
                {
                    if (numberOfShipments > 1000)
                    {
                        price = IncreasePriceForOver1000Shipments(result.Price, numberOfShipments);
                    }
                }
                if (notification.WasteComponentInfos.Count() > 0)
                {
                    var wasteComponentFees = await pricingFixedFeeRepository.GetAllWasteComponentFees(submittedDate);
                    if (wasteComponentFees.Count() > 0)
                    {
                        foreach (var wasteComponent in notification.WasteComponentInfos)
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

        private DateTimeOffset GetNotificationAssessmentSubmittedDate(NotificationAssessment notificationAssessment)
        {
            var submittedStatus = notificationAssessment.StatusChanges.FirstOrDefault(x => x.Status == NotificationStatus.Submitted);
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