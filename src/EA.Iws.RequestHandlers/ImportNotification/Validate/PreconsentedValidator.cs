namespace EA.Iws.RequestHandlers.ImportNotification.Validate
{
    using System.Threading;
    using System.Threading.Tasks;
    using Core.ImportNotification.Draft;
    using Core.Shared;
    using Domain.ImportNotification;
    using FluentValidation;

    internal class PreconsentedValidator : AbstractValidator<Preconsented>
    {
        private readonly IImportNotificationRepository importNotificationRepository;

        public PreconsentedValidator(IImportNotificationRepository importNotificationRepository)
        {
            this.importNotificationRepository = importNotificationRepository;

            RuleFor(x => x.AllFacilitiesPreconsented)
                .MustAsync(BeEnteredForRecoveryNotification)
                .WithLocalizedMessage(() => PreconsentedValidatorResources.PreconsentMustBeEntered);
        }

        private async Task<bool> BeEnteredForRecoveryNotification(Preconsented instance, bool? preconsentedFacilityExists, CancellationToken cancellationToken)
        {
            var importNotification =
                await importNotificationRepository.Get(instance.ImportNotificationId);

            return importNotification.NotificationType == NotificationType.Recovery
                ? preconsentedFacilityExists.HasValue
                : !preconsentedFacilityExists.HasValue;
        }
    }
}