namespace EA.Iws.RequestHandlers.ImportNotification.Validate
{
    using System;
    using Core.ImportNotification.Draft;
    using FluentValidation;

    public class FacilityValidator : AbstractValidator<Facility>
    {
        public FacilityValidator(IValidator<Address> addressValidator,
            IValidator<Contact> contactValidator)
        {
            RuleFor(x => x.Address)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .WithMessage(x => FacilityValidatorResources.AddressNotEmpty)
                .Must(BeNonEmptyAddress)
                .WithMessage(x => FacilityValidatorResources.AddressNotEmpty);

            RuleFor(x => x.Address)
                .SetValidator(addressValidator)
                .When(FacilityHasNonEmptyAddress);

            RuleFor(x => x.Contact)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .WithMessage(x => FacilityValidatorResources.ContactNotEmpty)
                .Must(BeNonEmptyContact)
                .WithMessage(x => FacilityValidatorResources.ContactNotEmpty);

            RuleFor(x => x.Contact)
                .SetValidator(contactValidator)
                .When(FacilityHasNonEmptyContact);

            RuleFor(x => x.BusinessName)
                .NotEmpty()
                .WithMessage(x => FacilityValidatorResources.BusinessNameNotEmpty);

            RuleFor(x => x.Type)
                .NotNull()
                .WithMessage(x => FacilityValidatorResources.BusinessTypeNotEmpty);
        }

        private bool FacilityHasNonEmptyContact(Facility facility)
        {
            return facility != null && BeNonEmptyContact(facility.Contact);
        }

        private bool BeNonEmptyContact(Contact contact)
        {
            return contact != null && !contact.IsEmpty;
        }

        private bool FacilityHasNonEmptyAddress(Facility facility)
        {
            return facility != null && BeNonEmptyAddress(facility.Address);
        }

        private bool BeNonEmptyAddress(Address address)
        {
            return address != null && !address.IsEmpty;
        }
    }
}