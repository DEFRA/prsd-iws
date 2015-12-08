namespace EA.Iws.RequestHandlers.ImportNotification.Validate
{
    using System;
    using System.Threading.Tasks;
    using Domain;
    using Domain.TransportRoute;
    using FluentValidation;
    using TransitState = Core.ImportNotification.Draft.TransitState;

    internal class TransitStateValidator : AbstractValidator<TransitState>
    {
        private readonly ICompetentAuthorityRepository competentAuthorityRepository;
        private readonly IEntryOrExitPointRepository entryOrExitPointRepository;

        public TransitStateValidator(IEntryOrExitPointRepository entryOrExitPointRepository,
            ICompetentAuthorityRepository competentAuthorityRepository)
        {
            this.competentAuthorityRepository = competentAuthorityRepository;
            this.entryOrExitPointRepository = entryOrExitPointRepository;

            RuleFor(x => x.CountryId)
                .NotNull()
                .WithLocalizedMessage(() => TransitStateValidatorResources.CountryNotNull);

            RuleFor(x => x.EntryPointId)
                .MustAsync(BeEnteringSameCountry)
                .WithLocalizedMessage(() => TransitStateValidatorResources.EntryPointMustBeSelectedCountry);

            RuleFor(x => x.ExitPointId)
                .MustAsync(BeExitingSameCountry)
                .WithLocalizedMessage(() => TransitStateValidatorResources.ExitPointMustBeSelectedCountry);

            RuleFor(x => x.CompetentAuthorityId)
                .MustAsync(BeInSameCountry)
                .WithLocalizedMessage(() => TransitStateValidatorResources.CompetentAuthorityMustBeSelectedCountry);
        }

        private async Task<bool> BeInSameCountry(TransitState transitState, Guid? competentAuthorityId)
        {
            if (competentAuthorityId.HasValue && transitState.CountryId.HasValue)
            {
                var competentAuthority =
                    await competentAuthorityRepository.GetById(competentAuthorityId.Value);

                return competentAuthority.Country.Id == transitState.CountryId.Value;
            }

            return false;
        }

        private async Task<bool> BeExitingSameCountry(TransitState transitState, Guid? exitPointId)
        {
            if (exitPointId.HasValue && transitState.CountryId.HasValue)
            {
                var exitPoint =
                    await entryOrExitPointRepository.GetById(exitPointId.Value);

                return exitPoint.Country.Id == transitState.CountryId.Value;
            }

            return false;
        }

        private async Task<bool> BeEnteringSameCountry(TransitState transitState, Guid? entryPointId)
        {
            if (entryPointId.HasValue && transitState.CountryId.HasValue)
            {
                var entryPoint =
                    await entryOrExitPointRepository.GetById(entryPointId.Value);

                return entryPoint.Country.Id == transitState.CountryId.Value;
            }

            return false;
        }
    }
}
