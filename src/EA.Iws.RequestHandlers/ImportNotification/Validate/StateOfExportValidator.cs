namespace EA.Iws.RequestHandlers.ImportNotification.Validate
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain;
    using Domain.TransportRoute;
    using FluentValidation;
    using StateOfExport = Core.ImportNotification.Draft.StateOfExport;

    internal class StateOfExportValidator : AbstractValidator<StateOfExport>
    {
        private readonly IEntryOrExitPointRepository entryOrExitPointRepository;
        private readonly ICompetentAuthorityRepository competentAuthorityRepository;

        public StateOfExportValidator(IEntryOrExitPointRepository entryOrExitPointRepository,
            ICompetentAuthorityRepository competentAuthorityRepository)
        {
            this.entryOrExitPointRepository = entryOrExitPointRepository;
            this.competentAuthorityRepository = competentAuthorityRepository;

            RuleFor(x => x.CountryId)
                .NotNull()
                .WithLocalizedMessage(() => StateOfExportValidatorResources.StateOfExportCountryNotNull);

            RuleFor(x => x.ExitPointId)
                .MustAsync(BeInSameCountry)
                .WithLocalizedMessage(() => StateOfExportValidatorResources.ExportExitPointMustBeInSameCountry);

            RuleFor(x => x.CompetentAuthorityId)
                .MustAsync(BeInCountry)
                .WithLocalizedMessage(() => StateOfExportValidatorResources.CompetentAuthorityMustBeInSameCountry);
        }

        private async Task<bool> BeInCountry(StateOfExport stateOfExport, Guid? competentAuthorityId, CancellationToken cancellationToken)
        {
            if (competentAuthorityId.HasValue && stateOfExport.CountryId.HasValue)
            {
                var competentAuthority = await competentAuthorityRepository.GetById(competentAuthorityId.Value);

                return competentAuthority.Country.Id == stateOfExport.CountryId.Value;
            }

            return false;
        }

        private async Task<bool> BeInSameCountry(StateOfExport stateOfExport, Guid? exitPointId, CancellationToken cancellationToken)
        {
            if (exitPointId.HasValue && stateOfExport.CountryId.HasValue)
            {
                var exitPoint = await entryOrExitPointRepository.GetById(exitPointId.Value);

                return exitPoint.Country.Id == stateOfExport.CountryId.Value;
            }

            return false;
        }
    }
}