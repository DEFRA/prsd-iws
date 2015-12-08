namespace EA.Iws.RequestHandlers.ImportNotification.Validate
{
    using System;
    using System.Threading.Tasks;
    using Domain;
    using Domain.TransportRoute;
    using FluentValidation;
    using StateOfImport = Core.ImportNotification.Draft.StateOfImport;

    internal class StateOfImportValidator : AbstractValidator<StateOfImport>
    {
        private readonly ICountryRepository countryRepository;
        private readonly ICompetentAuthorityRepository competentAuthorityRepository;
        private readonly IEntryOrExitPointRepository entryOrExitPointRepository;

        public StateOfImportValidator(ICountryRepository countryRepository,
            ICompetentAuthorityRepository competentAuthorityRepository,
            IEntryOrExitPointRepository entryOrExitPointRepository)
        {
            this.countryRepository = countryRepository;
            this.competentAuthorityRepository = competentAuthorityRepository;
            this.entryOrExitPointRepository = entryOrExitPointRepository;

            RuleFor(x => x.CompetentAuthorityId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .WithLocalizedMessage(() => StateOfImportValidatorResources.StateOfImportCompetentAuthorityNotEmpty)
                .MustAsync(BeACompetentAuthorityInTheUK)
                .WithLocalizedMessage(() => StateOfImportValidatorResources.StateOfImportCompetentAuthorityInUK);

            RuleFor(x => x.EntryPointId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .WithLocalizedMessage(() => StateOfImportValidatorResources.ImportEntryPointNotEmpty)
                .MustAsync(BeAnEntryPointInTheUK)
                .WithLocalizedMessage(() => StateOfImportValidatorResources.ImportEntryPointInUK);
        }

        private async Task<bool> BeAnEntryPointInTheUK(Guid? entryPointId)
        {
            var entryPoint = await entryOrExitPointRepository.GetById(entryPointId.Value);

            return await IsCountryUnitedKingdom(entryPoint.Country);
        }

        private async Task<bool> BeACompetentAuthorityInTheUK(Guid? competentAuthorityId)
        {
            var competentAuthority = await competentAuthorityRepository.GetById(competentAuthorityId.Value);

            return await IsCountryUnitedKingdom(competentAuthority.Country);
        }

        private async Task<bool> IsCountryUnitedKingdom(Country country)
        {
            var unitedKingdomId = await countryRepository.GetUnitedKingdomId();
            return country.Id == unitedKingdomId;
        }
    }
}