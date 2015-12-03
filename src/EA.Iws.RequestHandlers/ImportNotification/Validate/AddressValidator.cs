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

            RuleFor(x => x.AddressLine1).NotEmpty();
            RuleFor(x => x.TownOrCity).NotEmpty();
            RuleFor(x => x.CountryId).NotEmpty();
            RuleFor(x => x.PostalCode).MustAsync(BeNotEmptyWhenCountryIsUk);
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