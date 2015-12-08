namespace EA.Iws.RequestHandlers.ImportNotification.Validate
{
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
                .WithLocalizedMessage(() => AddressValidatorResources.AddressLine1NotEmpty);

            RuleFor(x => x.TownOrCity)
                .NotEmpty()
                .WithLocalizedMessage(() => AddressValidatorResources.TownAndCityNotEmpty);

            RuleFor(x => x.CountryId)
                .NotEmpty()
                .WithLocalizedMessage(() => AddressValidatorResources.CountryNotEmpty);

            RuleFor(x => x.PostalCode)
                .MustAsync(BeNotEmptyWhenCountryIsUk)
                .WithLocalizedMessage(() => AddressValidatorResources.PostcodeNotEmpty);
        }

        private async Task<bool> BeNotEmptyWhenCountryIsUk(Address address, string postCode)
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