namespace EA.Iws.RequestHandlers.ImportNotification.Validate
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.ImportNotification.Draft;
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
                .WithLocalizedMessage(() => ShipmentValidatorResources.TotalShipmentsNotNull)
                .GreaterThan(0)
                .WithLocalizedMessage(() => ShipmentValidatorResources.TotalShipmentsPositive);

            RuleFor(x => x.Quantity)
                .NotNull()
                .WithLocalizedMessage(() => ShipmentValidatorResources.QuantityNotNull)
                .GreaterThan(0)
                .WithLocalizedMessage(() => ShipmentValidatorResources.QuantityPositive);

            RuleFor(x => x.Unit)
                .NotNull()
                .WithLocalizedMessage(() => ShipmentValidatorResources.UnitNotNull);

            RuleFor(x => x.StartDate)
                .NotNull()
                .WithLocalizedMessage(() => ShipmentValidatorResources.StartDateNotNull)
                .LessThanOrEqualTo(s => s.EndDate)
                .WithLocalizedMessage(() => ShipmentValidatorResources.StartDateBeforeEndDate);

            RuleFor(x => x.EndDate)
                .NotNull()
                .WithLocalizedMessage(() => ShipmentValidatorResources.EndDateNotNull)
                .GreaterThanOrEqualTo(s => s.StartDate)
                .WithLocalizedMessage(() => ShipmentValidatorResources.EndDateAfterStartDate)
                .MustAsync(BeWithinConsentPeriod)
                .WithLocalizedMessage(() => ShipmentValidatorResources.EndDateInConsentPeriod);
        }

        private async Task<bool> BeWithinConsentPeriod(Shipment instance, DateTime? endDate, CancellationToken cancellationToken)
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