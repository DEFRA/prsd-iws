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
                .WithMessage(x => ShipmentValidatorResources.TotalShipmentsNotNull)
                .GreaterThan(0)
                .WithMessage(x => ShipmentValidatorResources.TotalShipmentsPositive);

            RuleFor(x => x.Quantity)
                .NotNull()
                .WithMessage(x => ShipmentValidatorResources.QuantityNotNull)
                .GreaterThan(0)
                .WithMessage(x => ShipmentValidatorResources.QuantityPositive);

            RuleFor(x => x.Unit)
                .NotNull()
                .WithMessage(x => ShipmentValidatorResources.UnitNotNull);

            RuleFor(x => x.StartDate)
                .NotNull()
                .WithMessage(x => ShipmentValidatorResources.StartDateNotNull)
                .LessThanOrEqualTo(s => s.EndDate)
                .WithMessage(x => ShipmentValidatorResources.StartDateBeforeEndDate);

            RuleFor(x => x.EndDate)
                .NotNull()
                .WithMessage(x => ShipmentValidatorResources.EndDateNotNull)
                .GreaterThanOrEqualTo(s => s.StartDate)
                .WithMessage(x => ShipmentValidatorResources.EndDateAfterStartDate)
                .MustAsync(BeWithinConsentPeriod)
                .WithMessage(x => ShipmentValidatorResources.EndDateInConsentPeriod);
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