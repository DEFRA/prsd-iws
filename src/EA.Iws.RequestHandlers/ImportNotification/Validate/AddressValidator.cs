namespace EA.Iws.RequestHandlers.ImportNotification.Validate
{
    using System.Threading;
    using System.Threading.Tasks;
    using Domain;
    using FluentValidation;
    using Address = Core.ImportNotification.Draft.Address;

    internal class AddressValidator : AbstractValidator<Address>
    {
        private readonly ICountryRepository repository;

        public AddressValidator(ICountryRepository repository)
        {
            this.repository = repository;

            RuleFor(x => x.AddressLine1)
                .NotEmpty()
                .WithMessage(AddressValidatorResources.AddressLine1NotEmpty);

            RuleFor(x => x.TownOrCity)
                .NotEmpty()
                .WithMessage(AddressValidatorResources.TownAndCityNotEmpty);

            RuleFor(x => x.CountryId)
                .NotEmpty()
                .WithMessage(AddressValidatorResources.CountryNotEmpty);

            RuleFor(x => x.PostalCode)
                .MustAsync(BeNotEmptyWhenCountryIsUk)
                .WithMessage(AddressValidatorResources.PostcodeNotEmpty);
        }

        private async Task<bool> BeNotEmptyWhenCountryIsUk(Address address, string postCode, CancellationToken cancellationToken)
        {
            var unitedKingdomId = await repository.GetUnitedKingdomId();

            if (address.CountryId.HasValue && address.CountryId.Value == unitedKingdomId)
            {
                return !string.IsNullOrWhiteSpace(postCode);
            }

            return true;
        }
    }
}