namespace EA.Iws.RequestHandlers.ImportNotification.Validate
{
    using System;
    using System.Threading.Tasks;
    using Core.ImportNotification.Draft;
    using Core.Shared;
    using DataAccess.Draft;
    using FluentValidation;

    internal class ShipmentValidator : AbstractValidator<Shipment>
    {
        private readonly IDraftImportNotificationRepository draftImportNotificationRepository;

        public ShipmentValidator(IDraftImportNotificationRepository draftImportNotificationRepository)
        {
            this.draftImportNotificationRepository = draftImportNotificationRepository;

            RuleFor(x => x.TotalShipments)
                .NotNull()
                .GreaterThan(0);

            RuleFor(x => x.Quantity)
                .NotNull()
                .GreaterThan(0);

            RuleFor(x => x.Unit)
                .NotNull();

            RuleFor(x => x.StartDate)
                .NotNull()
                .LessThanOrEqualTo(s => s.EndDate);

            RuleFor(x => x.EndDate)
                .NotNull()
                .GreaterThanOrEqualTo(s => s.StartDate)
                .MustAsync(BeWithinConsentPeriod);
        }

        private async Task<bool> BeWithinConsentPeriod(Shipment instance, DateTime? endDate)
        {
            var preconsented =
                await draftImportNotificationRepository.GetDraftData<Preconsented>(instance.ImportNotificationId);

            int maxPeriodLengthMonths = preconsented.AllFacilitiesPreconsented.GetValueOrDefault() ? 36 : 12;

            if (endDate >= instance.StartDate.GetValueOrDefault().AddMonths(maxPeriodLengthMonths))
            {
                return false;
            }

            return true;
        }
    }
}
